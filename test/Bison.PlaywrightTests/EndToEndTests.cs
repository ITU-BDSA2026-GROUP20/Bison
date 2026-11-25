using Microsoft.Playwright.NUnit;

namespace Bison.PlaywrightTests;

[TestFixture]
public class EndToEndTests : PageTest
{
    private BisonServer server = null!;

    [OneTimeSetUp]
    public async Task StartServer() => server = await BisonServer.StartAsync();

    [OneTimeTearDown]
    public async Task StopServer() => await server.DisposeAsync();
}