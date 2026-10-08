using System.Net;
using System.Net.Http.Json;
using Bison.Core.models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly DBServiceFactory factory;

    public DBServiceApiTests(DBServiceFactory factory)
    {
        this.factory = factory;
        client = factory.CreateClient();
    }

    private async Task<int> CreateAuthor()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Database>();
        var author = new Author { Name = $"user_{Guid.NewGuid():N}", Email = "test@test.dk" };
        db.Set<Author>().Add(author);
        await db.SaveChangesAsync();
        return author.Id;
    }

    private async Task PostObservation(int authorId, string text, string timestamp = "1700000000")
    {
        var observation = new Observation
        {
            AuthorId = authorId,
            Text = text,
            TimeStamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).UtcDateTime
        };
        var response = await client.PostAsJsonAsync("/observation", observation);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<List<Observation>> GetObs(string url)
    {
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<List<Observation>>() ?? new List<Observation>();
    }

    [Fact]
    public async Task PublicTimelineReturns()
    {
        await PostObservation(await CreateAuthor(), "A sparrow on a bench");

        var observations = await GetObs("/obs");

        Assert.NotEmpty(observations);
    }

    [Fact]
    public async Task PublicTimelineContainsPostedObservation()
    {
        int author = await CreateAuthor();
        await PostObservation(author, "A big gray bird in a pond at DR byen");

        var observations = await GetObs($"/obs?userId={author}");

        Assert.Contains(observations, o => o.Text == "A big gray bird in a pond at DR byen");
    }

    [Fact]
    public async Task PrivateTimelineOnlyContainsThatAuthorsObservations()
    {
        int petra = await CreateAuthor();
        int peter = await CreateAuthor();
        await PostObservation(petra, "A heron");
        await PostObservation(peter, "A fox");

        var observations = await GetObs($"/obs?userId={petra}");

        Assert.Single(observations);
        Assert.Equal("A heron", observations[0].Text);
        Assert.All(observations, o => Assert.Equal(petra, o.AuthorId));
    }

    [Fact]
    public async Task PaginationPagesHoldAtMost32Observations()
    {
        int author = await CreateAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", (1700000000 + i).ToString());

        var page1 = await GetObs($"/obs?userId={author}&page=1");
        var page2 = await GetObs($"/obs?userId={author}&page=2");

        Assert.Equal(32, page1.Count);
        Assert.Single(page2);
        Assert.Empty(page1.Select(o => o.Text).Intersect(page2.Select(o => o.Text)));
    }

    [Fact]
    public async Task PaginationNoPageParameterReturnsFirstPage()
    {
        int author = await CreateAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", (1700000000 + i).ToString());

        var withoutPage = await GetObs($"/obs?userId={author}");
        var withPage = await GetObs($"/obs?userId={author}&page=1");

        Assert.Equal(
            withPage.Select(o => o.Text),
            withoutPage.Select(o => o.Text));
    }
}