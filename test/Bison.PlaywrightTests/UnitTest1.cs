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
    
}