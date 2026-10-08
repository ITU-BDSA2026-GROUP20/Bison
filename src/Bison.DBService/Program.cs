using Bison.Core.models;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


var builder = WebApplication.CreateBuilder(args);

string dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? Path.Combine(Path.GetTempPath(), "bison.db");

var dir = Path.GetDirectoryName(dbPath);
if (!string.IsNullOrEmpty(dir))
{
    Directory.CreateDirectory(dir);
} else
{
    Console.Error.WriteLine("Invalid path given");
}

builder.Services.AddDbContext<Database>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

//Update on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Database>();
    db.Database.Migrate();
}

// endpoints
//GET
app.MapGet("/comments", async (int? id, IPostRepository repository) => 
{
    var comments = await repository.GetComments(id);
    return Results.Ok(comments);
});

app.MapGet("/proposals", async (int? id, IPostRepository repository) =>
{
    var proposals = await repository.GetProposals(id);
    return Results.Ok(proposals);
});

app.MapGet("/taxons", async (Database db) => await db.taxons.ToListAsync());

app.MapGet("/observations", async (int? id, IPostRepository repository) =>   // CHANGED
{
    if (id is null)
        return Results.Ok(await repository.GetObservations());

    var observation = await repository.GetObservation(id.Value);
    return observation is null ? Results.NotFound() : Results.Ok(observation);
});

app.MapGet("/obs", async (IPostRepository repository, int? userId, int page = 1) =>
{
    var observations = await repository.GetTimelineObservations(userId, page);
    return observations.Select(r=> new{r.Id, r.AuthorId, AuthorName = r.Author.Name, r.Text, r.TimeStamp}).ToList();
});

//POST
app.MapPost("/proposal", async (Proposal proposal, IPostRepository repository) =>
{
    await repository.AddProposal(proposal);
    return Results.Ok(proposal); 
});


app.MapPost("/comment", async (Comment comment, IPostRepository repository) =>
{
    await repository.AddComment(comment);
    return Results.Ok(comment);
});


app.MapPost("/observation", async (Observation observation, IPostRepository repository) =>
{
    await repository.AddObservation(observation);
    return Results.Ok(observation);
});

app.MapPost("/taxon", async (Taxon taxon, Database db) =>
{
    db.taxons.Add(taxon);
    await db.SaveChangesAsync();
    return Results.Ok(taxon);
});



app.Run();
 public partial class Program { }