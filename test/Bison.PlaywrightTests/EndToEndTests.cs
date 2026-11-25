using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Bison.PlaywrightTests;

[TestFixture]
public class EndToEndTests : PageTest
{
    private BisonServer server = null!;

    [OneTimeSetUp]
    public async Task StartServer() => server = await BisonServer.StartAsync();

    [OneTimeTearDown]
    public async Task StopServer() => await server.DisposeAsync();
    private async Task<string> RegisterNewUserAsync()
    {
        string email = $"e2e-{Guid.NewGuid():N}@test.dk";
        const string password = "Test1234!";

        await Page.GotoAsync($"{server.BaseUrl}/Identity/Account/Register");
        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        await Page.GetByLabel("Confirm Password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register", Exact = true }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Logout" })).ToBeVisibleAsync();
        return email;
    }
    private async Task<int> PostObservationAsync(string text, string? taxon = null)
    {
        await Page.GotoAsync($"{server.BaseUrl}/new");
        await Page.Locator("#Text").FillAsync(text);
        if (taxon is not null)
            await Page.Locator("#TaxonId").SelectOptionAsync(new SelectOptionValue { Label = taxon });
        await Page.GetByRole(AriaRole.Button, new() { Name = "Post observation" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(@"/ob/\d+$"));
        return int.Parse(Page.Url.Split('/').Last());
    }

    [Test]
    public async Task PostedObservationIsStoredInTheDatabaseForTheLoggedInAuthor()
    {
        // Arrange
        string email = await RegisterNewUserAsync();
        string text = $"Grey heron at Utterslev Mose {Guid.NewGuid():N}";

        // Act
        int id = await PostObservationAsync(text, taxon: "Fiskehejre");

        // Assert
        using var db = server.OpenDatabase();
        var stored = await db.readings.AsNoTracking()
            .Include(o => o.Author)
            .Include(o => o.Taxon)
            .SingleAsync(o => o.Id == id);

        Assert.That(stored.Text, Is.EqualTo(text));
        Assert.That(stored.Author.Email, Is.EqualTo(email));
        Assert.That(stored.Taxon?.VernacularName, Is.EqualTo("Fiskehejre"));

        var user = await db.Users.AsNoTracking().SingleAsync(u => u.Email == email);
        Assert.That(user.AuthorId, Is.EqualTo(stored.AuthorId));
    }
}