using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
namespace Bison.PlaywrightTests;

[TestFixture]
public class Tests : PageTest
{
    private BisonServer server = null!;

    [OneTimeSetUp]
    public async Task StartBison() => server = await BisonServer.StartAsync();

    [OneTimeTearDown]
    public async Task StopBison()
    {
        if (server is not null) await server.DisposeAsync();
    }
    private async Task Login()
    {
        string email = $"ui-{Guid.NewGuid():N}@test.dk";
        await Page.GotoAsync($"{server.BaseUrl}/Identity/Account/Register");
        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test1234!");
        await Page.GetByLabel("Confirm Password").FillAsync("Test1234!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register", Exact = true }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Logout" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task MyObservationsHiddenWhenLoggedOut()
    {
        await Page.GotoAsync(server.BaseUrl);
        var myObservations = Page.GetByRole(AriaRole.Link, 
        new() { Name = "My Observations" });
        await Expect(myObservations).ToBeHiddenAsync();
    }

    [Test]
    public async Task PublicObservationsVisibleWhenLoggedOut()
    {
       await Page.GotoAsync(server.BaseUrl);
       var PublicObservations = Page.GetByRole(AriaRole.Link,
       new() { Name = "Public Observations" });
       await Expect(PublicObservations).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task RegisterVisibleWhenLoggedOut()
    {
       await Page.GotoAsync(server.BaseUrl);
       var Register = Page.GetByRole(AriaRole.Link,
       new() { Name = "Register" });
       await Expect(Register).ToBeVisibleAsync(); 
    }

     [Test]
    public async Task LoginVisibleWhenLoggedOut()
    {
       await Page.GotoAsync(server.BaseUrl);
       var Login = Page.GetByRole(AriaRole.Link,
       new() { Name = "Login" });
       await Expect(Login).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task TaxonFilterVisibleWhenLoggedOut()
    {
       await Page.GotoAsync(server.BaseUrl);
       var taxonFilter = Page.GetByRole(AriaRole.Combobox); 
       await Expect(taxonFilter).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task TaxonFilterSelectOption()
    {
       await Page.GotoAsync(server.BaseUrl);
       var taxonFilter = Page.GetByRole(AriaRole.Combobox); 
       await taxonFilter.SelectOptionAsync(new SelectOptionValue { Label = "Amerikansk rørdrum" });
       await Expect(taxonFilter.Locator("option:checked")).ToHaveTextAsync("Amerikansk rørdrum");
       await Expect(Page.GetByText(
        "One American bittern passing a wet meadow heading south.",
         new() { Exact = false })).ToBeVisibleAsync();
        await Expect(Page.GetByText(
            "Grey heron standing still in the reeds at the lake.", 
            new() { Exact = false })).ToBeHiddenAsync();
    }

    [Test]
    public async Task LoginLinkNavigatesToLoginPage()
    {
        await Page.GotoAsync(server.BaseUrl);
        var loginLink = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });
        await loginLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync($"{server.BaseUrl}/Identity/Account/Login");
    }

    [Test]
    public async Task RegisterlinkNavigatesToRegisterPage()
    {
        await Page.GotoAsync(server.BaseUrl);
        var registerLink = Page.GetByRole(AriaRole.Link, new() { Name = "Register" });
        await registerLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync($"{server.BaseUrl}/Identity/Account/Register");
    }

    [Test]
    public async Task NewObservationRequiresLogin()
    {
        await Page.GotoAsync($"{server.BaseUrl}/new");
        Assert.That(Page.Url, Does.Contain("/Identity/Account/Login"));
    }
    //test efter login 
    [Test]
    public async Task MyObservationsVisibleWhenLoggedIn()
    {
        await Login();
        await Page.GotoAsync($"{server.BaseUrl}/new");
        await Page.Locator("#Text").FillAsync("My first observation");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Post observation" }).ClickAsync();

        var myObservations = Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" });
        await Expect(myObservations).ToBeVisibleAsync();
    }

    [Test]
    public async Task PublicObservationsVisibleWhenLoggedin()
    {
       await Login();
       var PublicObservations = Page.GetByRole(AriaRole.Link,
       new() { Name = "Public Observations" });
       await Expect(PublicObservations).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task LogoutVisibleWhenLoggedIn()
    {
       await Login();
       var Logout = Page.GetByRole(AriaRole.Button,
       new() { Name = "Logout" });
       await Expect(Logout).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task NewObservationPageOpensWhenLoggedIn()
    {
       await Login();
       var newObservationLink = Page.GetByRole(AriaRole.Link,
       new() { Name = "New Observation" });
         await newObservationLink.ClickAsync();
         var observationText = Page.Locator("textarea");
         await Expect(observationText).ToBeVisibleAsync();
    }

    [Test]
    public async Task ObservationTextMaxLenght()
    {
        await Login();
        var newObservationLink = Page.GetByRole(AriaRole.Link,
        new() { Name = "New Observation" });
        await newObservationLink.ClickAsync();
        var observationText = Page.Locator("textarea");
        string longText = new string('a', 501);
        await observationText.FillAsync(longText);
        await Expect(observationText).ToHaveValueAsync(longText.Substring(0, 500));
    }

    [Test]
    public async Task ObservationNoText()
    {
        await Login();
        var newObservationLink = Page.GetByRole(AriaRole.Link,
        new() { Name = "New Observation" });
        await newObservationLink.ClickAsync();
        var observationText = Page.Locator("textarea");
        await Page.GetByRole(AriaRole.Button, new() { 
            Name = "Post observation" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync($"{server.BaseUrl}/new");
    }

    [Test]
    public async Task ObservationTaxonSelection()
    {
        await Login();
        await Page.GetByRole(AriaRole.Link, 
            new() { Name = "New Observation" }).ClickAsync();
        var taxon = Page.GetByRole(AriaRole.Combobox);
        await taxon.SelectOptionAsync(new SelectOptionValue 
            { Label = "Amerikansk rørdrum" });
        await Expect(taxon.Locator("option:checked"))
            .ToHaveTextAsync("Amerikansk rørdrum");
    }

    [Test]
    public async Task EmptyObservationShowsError()
    {
        await Login();
        await Page.GetByRole(AriaRole.Link, 
            new() { Name = "New Observation" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, 
            new() { Name = "Post observation" }).ClickAsync();
        await Expect(Page.GetByText("The Text field is required."))
            .ToBeVisibleAsync();
    }

    [Test]
    public async Task LogoutWorks()
    {
        await Login();
        await Page.GetByRole(AriaRole.Button, 
            new() { Name = "Logout" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Link, 
            new() { Name = "Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, 
            new() { Name = "My Observations" })).ToBeHiddenAsync();
    }
}