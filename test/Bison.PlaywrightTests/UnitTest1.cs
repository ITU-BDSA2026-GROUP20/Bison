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
}