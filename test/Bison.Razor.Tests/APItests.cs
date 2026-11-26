using Bison.Core.models;
using Bison.Infrastructure;
using Bison.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bison.Razor.Tests;

// InMemoryDb replaces DBServiceFactory. 
// A fresh in-memory SQLite database instead of starting the whole server with a temp .db file.
// Maybe this should be changed, but not sure how else to do it?.
public sealed class InMemoryDb : IDisposable
{
    private readonly SqliteConnection connection;
    public Database Db { get; }

    public InMemoryDb()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open(); // must stay open, or the database disappears
        var options = new DbContextOptionsBuilder<Database>().UseSqlite(connection).Options;
        Db = new Database(options);
        Db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Db.Dispose();
        connection.Dispose();
    }
}
public class APITests : IDisposable
{
    private readonly InMemoryDb mem = new();
    private readonly ObservationRepository observationRepository;
    private readonly AuthorRepository authorRepository;
    private readonly ObservationService service;
    public APITests()
    {
        observationRepository = new ObservationRepository(mem.Db);
        authorRepository = new AuthorRepository(mem.Db);
        service = new ObservationService(
            observationRepository,
            new CommentRepository(mem.Db),
            new ProposalRepository(mem.Db),
            new TaxonRepository(mem.Db),
            authorRepository,
            new BattlePassRepository(mem.Db));
    }
    public void Dispose() => mem.Dispose();
    private async Task<int> CreateAuthor()
    {
        var author = new Author { Name = $"user_{Guid.NewGuid():N}", Email = $"{Guid.NewGuid():N}@test.dk" };
        mem.Db.users.Add(author);
        await mem.Db.SaveChangesAsync();
        return author.Id;
    }
    private Task PostObservation(int authorId, string text, long unixSeconds = 1700000000)
        => observationRepository.AddObservation(new Observation
        {
            AuthorId = authorId,
            Text = text,
            TimeStamp = DateTime.UnixEpoch.AddSeconds(unixSeconds)
        });

    [Fact]
    public async Task PublicTimelineReturns()
    {
        await PostObservation(await CreateAuthor(), "A sparrow on a bench");

        var observations = service.GetObservations();

        Assert.NotEmpty(observations);
    }

    [Fact]
    public async Task PublicTimelineContainsPostedObservation()
    {
        int author = await CreateAuthor();
        await PostObservation(author, "A big gray bird in a pond at DR byen");

        var observations = service.GetObservations(author);

        Assert.Contains(observations, o => o.Text == "A big gray bird in a pond at DR byen");
    }

    [Fact]
    public async Task PrivateTimelineOnlyContainsThatAuthorsObservations()
    {
        int petra = await CreateAuthor();
        int peter = await CreateAuthor();
        await PostObservation(petra, "A heron");
        await PostObservation(peter, "A fox");

        var observations = service.GetObservations(petra);

        Assert.Single(observations);
        Assert.Equal("A heron", observations[0].Text);
        Assert.All(observations, o => Assert.Equal(petra, o.AuthorId));
    }

    [Fact]
    public async Task PaginationPagesHoldAtMost32Observations()
    {
        int author = await CreateAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", 1700000000 + i);

        var page1 = service.GetObservations(author, 1);
        var page2 = service.GetObservations(author, 2);

        Assert.Equal(32, page1.Count);
        Assert.Single(page2);
        Assert.Empty(page1.Select(o => o.Text).Intersect(page2.Select(o => o.Text)));
    }

    [Fact]
    public async Task PaginationNoPageParameterReturnsFirstPage()
    {
        int author = await CreateAuthor();
        for (int i = 0; i < 33; i++)
            await PostObservation(author, $"Observation {i}", 1700000000 + i);

        var withoutPage = service.GetObservations(author);
        var withPage = service.GetObservations(author, 1);

        Assert.Equal(withPage.Select(o => o.Text), withoutPage.Select(o => o.Text));
    }

    [Fact]
    public async Task AddObservationWithValidDtoReturnsObservation()
    {
        var dto = new CreateObservationDTO(
            Text: "Example",
            AuthorName: "exampleAuthor",
            AuthorEmail: "example@test.dk");

        var created = await service.AddObservation(dto);

        Assert.NotNull(created);
        Assert.Equal("Example", created.Text);
        Assert.Equal("exampleAuthor", created.AuthorName);
        Assert.Null(created.TaxonId);
    }

    [Fact]
    public async Task AddObservationCreatesAuthorAndShowsUpInTimeline()
    {
        var dto = new CreateObservationDTO(
            Text: "example text",
            AuthorName: "Woody",
            AuthorEmail: "woody@test.dk");

        var created = await service.AddObservation(dto);
        var observations = service.GetObservations(created!.AuthorId);

        Assert.Single(observations);
        Assert.Equal("example text", observations[0].Text);
    }
    
    [Fact]
    public async Task GetOrCreateAuthorReusesAuthorWithSameEmail()
    {
        var first = await authorRepository.GetOrCreateAuthor(null, "Same Person", "same@test.dk");
        var second = await authorRepository.GetOrCreateAuthor(null, "Same Person", "same@test.dk");

        Assert.Equal(first.Id, second.Id);
    }

    [Fact]
    public async Task GetOrCreateAuthorUsesExistingAuthorWhenIdGiven()
    {
        int authorId = await CreateAuthor();

        var author = await authorRepository.GetOrCreateAuthor(authorId, "Ignored Name", "ignored@test.dk");

        Assert.Equal(authorId, author.Id);
    }
}