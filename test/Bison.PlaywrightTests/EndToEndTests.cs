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

    [Test]
        public async Task ObservationsAreStoredForTheirOwnAuthor()
    {
        string sean = await RegisterNewUserAsync();
        await PostObservationAsync("Sean saw a spoonbill");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();

        string mikkel = await RegisterNewUserAsync();
        await PostObservationAsync("Mikkel saw a cormorant");

        using var db = server.OpenDatabase();
        var seanTexts = await db.readings.Where(o => o.Author.Email == sean).Select(o => o.Text).ToListAsync();
        var mikkelTexts = await db.readings.Where(o => o.Author.Email == mikkel).Select(o => o.Text).ToListAsync();

        Assert.That(seanTexts, Is.EqualTo(new[] { "Sean saw a spoonbill" }));
        Assert.That(mikkelTexts, Is.EqualTo(new[] { "Mikkel saw a cormorant" }));
    }
    [Test]
    public async Task PostedObservationIsShownOnItsPageAndInMyObservations()
    {
        await RegisterNewUserAsync();
        string text = $"Little egret in the harbour {Guid.NewGuid():N}";

        await PostObservationAsync(text);
        await Expect(Page.GetByText(text)).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" }).ClickAsync();
        await Expect(Page.GetByText(text)).ToBeVisibleAsync();
    }
    [Test]
    public async Task EmptyObservationIsNotStored()
    {
        string email = await RegisterNewUserAsync();

        await Page.GotoAsync($"{server.BaseUrl}/new");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Post observation" }).ClickAsync();

        await Expect(Page.GetByText("The Text field is required.")).ToBeVisibleAsync();
        await Expect(Page).ToHaveURLAsync($"{server.BaseUrl}/new");

        using var db = server.OpenDatabase();
        Assert.That(await db.readings.CountAsync(o => o.Author.Email == email), Is.Zero);
        Assert.That(await db.users.AnyAsync(a => a.Email == email), Is.False);
    }
    [Test]
    public async Task CommentIsStoredOnTheObservationForTheAuthor()
    {
        string email = await RegisterNewUserAsync();
        string text = $"Nice find! {Guid.NewGuid():N}";

        await Page.GotoAsync($"{server.BaseUrl}/ob/1");
        await Page.Locator("#CommentText").FillAsync(text);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Comment", Exact = true }).ClickAsync();
        await Expect(Page.GetByText(text)).ToBeVisibleAsync();

        using var db = server.OpenDatabase();
        var comment = await db.comments.AsNoTracking()
            .Include(c => c.Author)
            .SingleAsync(c => c.Text == text);

        Assert.That(comment.ObservationId, Is.EqualTo(1));
        Assert.That(comment.Author.Email, Is.EqualTo(email));
    }
}