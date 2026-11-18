using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Diagnostics;
using Microsoft.Playwright;
namespace Bison.PlaywrightTests;

[TestFixture]
public class Tests : PageTest
{
    private Process? bisonProcess;
    [OneTimeSetUp]
    public async Task StartBison()
    {
        string projectPath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../../src/Bison.Razor/Bison.Razor.csproj"));
        bisonProcess = Process.Start("dotnet",
            $"run --project \"{projectPath}\" --launch-profile http"
        );
        await Task.Delay(5000); 
    }

    [OneTimeTearDown]
    public void StopBison()
    {
        if (bisonProcess is {HasExited: false })
        {
            bisonProcess.Kill(entireProcessTree: true);
        }
        bisonProcess?.Dispose();
    }

    private async Task Login()
    {
        await Page.GotoAsync("http://localhost:5273/Identity/Account/Login");
        await Page.GetByRole(AriaRole.Textbox, new() {Name = "Email"})
        .FillAsync("bisontest@example.com"); 
        await Page.GetByLabel("Password").FillAsync("123456");
        await Page.GetByRole(AriaRole.Button, new() {Name = "Log in"})
        .ClickAsync();
    }

    [Test]
    public async Task MyObservationsHiddenWhenLoggedOut()
    {
        await Page.GotoAsync("http://localhost:5273");
        var myObservations = Page.GetByRole(AriaRole.Link, 
        new() { Name = "My Observations" });
        await Expect(myObservations).ToBeHiddenAsync();
    }

    [Test]
    public async Task PublicObservationsVisibleWhenLoggedOut()
    {
       await Page.GotoAsync("http://localhost:5273");
       var PublicObservations = Page.GetByRole(AriaRole.Link,
       new() { Name = "Public Observations" });
       await Expect(PublicObservations).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task RegisterVisibleWhenLoggedOut()
    {
       await Page.GotoAsync("http://localhost:5273");
       var Register = Page.GetByRole(AriaRole.Link,
       new() { Name = "Register" });
       await Expect(Register).ToBeVisibleAsync(); 
    }

     [Test]
    public async Task LoginVisibleWhenLoggedOut()
    {
       await Page.GotoAsync("http://localhost:5273");
       var Login = Page.GetByRole(AriaRole.Link,
       new() { Name = "Login" });
       await Expect(Login).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task TaxonFilterVisibleWhenLoggedOut()
    {
       await Page.GotoAsync("http://localhost:5273");
       var taxonFilter = Page.GetByRole(AriaRole.Combobox); 
       await Expect(taxonFilter).ToBeVisibleAsync(); 
    }

    [Test]
    public async Task TaxonFilterSelectOption()
    {
       await Page.GotoAsync("http://localhost:5273");
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
        await Page.GotoAsync("http://localhost:5273");
        var loginLink = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });
        await loginLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync("http://localhost:5273/Identity/Account/Login");
    }

    [Test]
    public async Task RegisterlinkNavigatesToRegisterPage()
    {
        await Page.GotoAsync("http://localhost:5273");
        var registerLink = Page.GetByRole(AriaRole.Link, new() { Name = "Register" });
        await registerLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync("http://localhost:5273/Identity/Account/Register");
    }

    [Test]
    public async Task NewObservationRequiresLogin()
    {
        await Page.GotoAsync("http://localhost:5273/new");
        Assert.That(Page.Url, Does.Contain("/Identity/Account/Login"));
    }
    //test efter login 
    [Test]
    public async Task MyObservationsVisibleWhenLoggedIn()
    {
        await Login();
        var myObservations = Page.GetByRole(AriaRole.Link,
         new() { Name = "My Observations" });
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
        await Expect(Page).ToHaveURLAsync("http://localhost:5273/new");
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
}