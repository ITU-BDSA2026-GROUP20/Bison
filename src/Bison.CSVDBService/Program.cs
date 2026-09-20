using SimpleDB;
using Bison.Core.models;
using Bison.CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CSVDatabase csvDatabase = CSVDatabase.Instance;
IDatabaseRepository<Reading> readingDatabase = csvDatabase.GetRepository<Reading>("reading");
IDatabaseRepository<Comment> commentDatabase = csvDatabase.GetRepository<Comment>("comment");
IDatabaseRepository<Proposal> proposalDatabase = csvDatabase.GetRepository<Proposal>("proposal");
TaxonRepository taxonRepository = new TaxonRepository("TaxonCsvDatabase.csv");

//Gets
app.MapGet("/observations", () => readingDatabase.Read().ToList());

app.MapGet("/comments", (Guid guid) =>
{
    List<Comment> comments = commentDatabase.Read().ToList();
    return comments.Where(c => c.Id == guid).ToList();
});
app.MapGet("/proposals", (Guid guid) =>
{
    List<Proposal> proposals = proposalDatabase.Read().ToList();
    return proposals.Where(p => p.Id == guid).ToList();
});

//Post
app.MapPost("/observation", (Reading reading) =>
{
    readingDatabase.Store(reading);
    return Results.Ok(true);
});

app.MapPost("/comment", (Comment comment) =>
{
    if(readingDatabase.Read().ToList().Find(r => r.Id == comment.Id) is null)
        return Results.BadRequest("The specified observation does not exist.");

    commentDatabase.Store(comment);
    return Results.Ok(true);
});

app.MapPost("/proposal", (Proposal proposal) =>
{
    if (taxonRepository.GetByID(proposal.TaxonID) is null)
        return Results.BadRequest("The specified taxon does not exist.");

    if(readingDatabase.Read().ToList().Find(r => r.Id == proposal.Id) is null)
        return Results.BadRequest("The specified observation does not exist.");

    proposalDatabase.Store(proposal);
    return Results.Ok(true);
});

app.Run();

// Makes Program accessible to WebApplicationFactory in integration tests
public partial class Program { }