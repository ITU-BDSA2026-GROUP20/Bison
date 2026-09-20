using SimpleDB;
using Bison.Core.models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CSVDatabase csvDatabase = CSVDatabase.Instance;
IDatabaseRepository<Reading> readingDatabase = csvDatabase.GetRepository<Reading>("reading");
IDatabaseRepository<Comment> commentDatabase = csvDatabase.GetRepository<Comment>("comment");
IDatabaseRepository<Proposal> proposalDatabase = csvDatabase.GetRepository<Proposal>("proposal");


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
    return reading;
});

app.MapPost("/comment", (Comment comment) =>
{
    commentDatabase.Store(comment);
    return comment;
});

app.MapPost("/proposal", (Proposal proposal) =>
{
    proposalDatabase.Store(proposal);
    return proposal;
});

app.Run();

// Makes Program accessible to WebApplicationFactory in integration tests
public partial class Program { }