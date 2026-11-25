using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System.Text.RegularExpressions;

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
}