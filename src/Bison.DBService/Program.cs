using Bison.Core.models;
using System.Data;
using Microsoft.Data.Sqlite;
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

builder.Services.AddDbContext<Database>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

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
app.MapGet("/comments", async (Database db) => await db.comments.ToListAsync());
app.MapGet("/proposals", async (Database db) => await db.proposals.ToListAsync());
app.MapGet("/taxons", async (Database db) => await db.taxons.ToListAsync());

app.MapGet("/observations", async (Guid? guid, Database db) =>
{
    if(guid is null)
        return Results.Ok(await db.readings.ToListAsync());

    var reading = await db.readings.FindAsync(guid.Value);
    return reading is null ? Results.NotFound() : Results.Ok(reading);
});


app.MapGet("/obs", async (Database db, string? author, int page = 1) =>
{
    IQueryable<Reading> q = db.readings;
    if (author is not null)
        q = q.Where(r => r.Author == author);

    return await q.OrderBy(r => r.Timestamp).Skip((page - 1) * 32).Take(32).ToListAsync();
});

//POST
app.MapPost("/proposal", async (Proposal proposal, Database db) =>
{
    db.proposals.Add(proposal);
    await db.SaveChangesAsync();
    return Results.Ok(proposal);
});


app.MapPost("/comment", async (Comment comment, Database db) =>
{
    db.comments.Add(comment);
    await db.SaveChangesAsync();
    return Results.Ok(comment);
});


app.MapPost("/observation", async (Reading reading, Database db) =>
{
    db.readings.Add(reading);
    await db.SaveChangesAsync();
    return Results.Ok(reading);
});

app.MapPost("/taxon", async (Taxon taxon, Database db) =>
{
    db.taxons.Add(taxon);
    await db.SaveChangesAsync();
    return Results.Ok(taxon);
});



app.Run();