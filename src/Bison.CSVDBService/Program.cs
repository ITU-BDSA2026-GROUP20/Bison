using Bison.CSVDBService.models;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CSVDatabase csvDatabase = CSVDatabase.Instance;
IDatabaseRepository<Reading> readingDatabase = csvDatabase.GetRepository<Reading>("reading");
IDatabaseRepository<Comment> commentDatabase = csvDatabase.GetRepository<Comment>("comment");


//Gets
app.MapGet("/observations", () => readingDatabase.Read().ToList());

app.MapGet("/comments", (Guid guid) =>
{
    List<Comment> comments = commentDatabase.Read().ToList();
    return comments.Where(c => c.Id == guid).ToList();
});

//Post
app.MapPost("/observation", (Reading reading) =>
{
    readingDatabase.Store(reading);
});

app.MapPost("/comment", (Comment comment) =>
{
    commentDatabase.Store(comment);
});

app.Run();

// Makes Program accessible to WebApplicationFactory in integration tests
public partial class Program { }