using Bison.Core.models;
using Bison.Infrastructure;
using Bison.Core.Repositories;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddBisonInfrastructure($"Data Source={dbPath}");

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

// endpoints
//GET
app.MapGet("/comments", async (int? id, ICommentRepository repository) => 
{
    var comments = await repository.GetComments(id);
    return Results.Ok(comments);
});

app.MapGet("/proposals", async (int? id, IProposalRepository repository) =>
{
    var proposals = await repository.GetProposals(id);
    return Results.Ok(proposals);
});

app.MapGet("/taxons", async (ITaxonRepository repository) => 
{
    var taxons = await repository.GetTaxons();
    return Results.Ok(taxons);
});
app.MapGet("/taxon", async (int id, ITaxonRepository repository) => 
{
    var taxon = await repository.GetTaxon(id);
    return taxon is null ? Results.NotFound() : Results.Ok(taxon);
});

app.MapGet("/allobs", async (IObservationRepository repository) =>
{
    var allObs = await repository.GetObservations();
    return Results.Ok(allObs);
});

app.MapGet("/observations", async (int? id, IObservationRepository repository) =>   // CHANGED
{
    if (id is null)
        return Results.Ok(await repository.GetObservations());

    var observation = await repository.GetObservation(id.Value);
    return observation is null ? Results.NotFound() : Results.Ok(observation);
});

app.MapGet("/obs", async (IObservationRepository repository, int? userId, int page = 1) =>
{
    var observations = await repository.GetTimelineObservations(userId, page);
    return observations.Select(o => new ObservationDTO(
    o.Id, o.Text, o.AuthorId, o.Author.Name, o.TimeStamp.ToString("yyyy-MM-dd"), o.TaxonId, o.Taxon?.VernacularName ?? string.Empty)).ToList();
});

//POST
app.MapPost("/proposal", async (Proposal proposal, IProposalRepository repository) =>
{
    await repository.AddProposal(proposal);
    return Results.Ok(proposal); 
});


app.MapPost("/comment", async (Comment comment, ICommentRepository repository) =>
{
    await repository.AddComment(comment);
    return Results.Ok(comment);
});


app.MapPost("/observation", async (Observation observation, IObservationRepository repository) =>
{
    await repository.AddObservation(observation);
    return Results.Ok(observation);
});

app.MapPost("/observations", async (CreateObservationDTO dto, Database db, IObservationRepository repository) =>
{
    if (string.IsNullOrWhiteSpace(dto.Text) || dto.Text.Length > 500)
        return Results.BadRequest("Text is required and must be at most 500 characters.");
    if (string.IsNullOrWhiteSpace(dto.AuthorEmail) || string.IsNullOrWhiteSpace(dto.AuthorName))
        return Results.BadRequest("Author name and email are required.");

    Taxon? taxon = null;
    if (dto.TaxonId is int taxonId)
    {
        taxon = await db.taxons.FindAsync(taxonId);
        if (taxon is null)
            return Results.BadRequest("Unknown taxon.");
    }

    Author? author = dto.AuthorId is int authorId ? await db.users.FindAsync(authorId) : null;
    author ??= await db.users.FirstOrDefaultAsync(a => a.Email == dto.AuthorEmail);
    if (author is null)
    {
        author = new Author { Name = dto.AuthorName, Email = dto.AuthorEmail };
        db.users.Add(author);
        await db.SaveChangesAsync();
    }

    var observation = await repository.AddObservation(new Observation
    {
        Text = dto.Text.Trim(),
        TimeStamp = DateTime.UtcNow,
        AuthorId = author.Id,
        TaxonId = taxon?.TaxonId
    });

    return Results.Ok(new ObservationDTO(
        observation.Id, observation.Text, author.Id, author.Name,
        observation.TimeStamp.ToString("yyyy-MM-dd"), taxon?.TaxonId, taxon?.VernacularName));
});

app.MapPost("/taxon", async (Taxon taxon, Database db) =>
{
    db.taxons.Add(taxon);
    await db.SaveChangesAsync();
    return Results.Ok(taxon);
});

app.MapGet("/seed", async (DatabaseInitializer initializer) =>
{
    await initializer.InitializeAsync();
    return Results.Ok();
});

app.Run();
 public partial class Program { }