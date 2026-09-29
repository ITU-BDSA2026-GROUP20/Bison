using System.Net;
using System.Net.Http.Json;
using Bison.Core.models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace Bison.Razor.Tests;
public class DBServiceFactory : WebApplicationFactory<Program>
{
    private readonly string dbPath =
        Path.Combine(Path.GetTempPath(), $"bison_test_{Guid.NewGuid()}.db");

    public DBServiceFactory()
    {
        Environment.SetEnvironmentVariable("BISONDBPATH", dbPath);
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        Environment.SetEnvironmentVariable("BISONDBPATH", null);
        SqliteConnection.ClearAllPools();
        File.Delete(dbPath);
    }
}
public class DBServiceApiTests : IClassFixture<DBServiceFactory>
{
    private readonly HttpClient client;

    public DBServiceApiTests(DBServiceFactory factory)
    {
        client = factory.CreateClient();
    }
    private static string UniqueAuthor() => $"author_{Guid.NewGuid():N}";
    private async Task PostObservation(string author, string text, string timestamp = "1700000000")
    {
        var response = await client.PostAsJsonAsync("/observation", new Reading(author, text, timestamp));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    private async Task<List<Reading>> GetObs(string url)
    {
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<List<Reading>>() ?? new List<Reading>();
    }

    [Fact]
    public async Task PublicTimelineReturns()
    {
        var observations = await GetObs("/obs");

        Assert.NotNull(observations);
    }

    [Fact]
    public async Task PublicTimelineContainsPostedObservation()
    {
        string author = UniqueAuthor();
        await PostObservation(author, "A big gray bird in a pond at DR byen");

        var observations = await GetObs($"/obs?author={author}");

        Assert.Contains(observations, o => o.Observation == "A big gray bird in a pond at DR byen");
    }

    [Fact]
    public async Task PrivateTimelineOnlyContainsThatAuthorsObservations()
    {
        string petra = UniqueAuthor();
        string peter = UniqueAuthor();
        await PostObservation(petra, "A heron");
        await PostObservation(peter, "A fox");

        var observations = await GetObs($"/obs?author={petra}");

        Assert.Single(observations);
        Assert.Equal("A heron", observations[0].Observation);
        Assert.All(observations, o => Assert.Equal(petra, o.Author));
    }

    [Fact]
    public async Task PaginationPagesHoldAtMost32Observations()
    {
        string author = UniqueAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", (1700000000 + i).ToString());

        var page1 = await GetObs($"/obs?author={author}&page=1");
        var page2 = await GetObs($"/obs?author={author}&page=2");

        Assert.Equal(32, page1.Count);
        Assert.Single(page2);
        Assert.Empty(page1.Select(o => o.Observation).Intersect(page2.Select(o => o.Observation)));
    }

    [Fact]
    public async Task PaginationNoPageParameterReturnsFirstPage()
    {
        string author = UniqueAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", (1700000000 + i).ToString());

        var withoutPage = await GetObs($"/obs?author={author}");
        var withPage = await GetObs($"/obs?author={author}&page=1");

        Assert.Equal(
            withPage.Select(o => o.Observation),
            withoutPage.Select(o => o.Observation));
    }
}