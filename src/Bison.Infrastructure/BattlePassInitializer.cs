using Bison.Core.models;
using Microsoft.EntityFrameworkCore;
using Bison.Core.models.BattlePass;

namespace Bison.Infrastructure;

public sealed class BattlePassInitializer(Database database)
{
    public async Task InitializeAsync()
    {
        await database.Database.MigrateAsync();

        if (!database.users.Any() || !database.readings.Any())
        {
            SeedDatabase();

            await database.SaveChangesAsync();
        }
    }

    private void SeedDatabase()
    {
        if (!(database.users.Any() && database.readings.Any()))
        {
            var tier1 = new Unlock() {};
            var tier2 = new ReSkin() {PrimaryColor = 0x2E5266, SecondaryColor = 0x6E8898, BackgroundColor =  0x9FB1BC};
            var tier7  = new ReSkin() { PrimaryColor = 0x2D4A2B, SecondaryColor = 0x6B8E4E, BackgroundColor = 0xA9C48C };
            var tier12 = new ReSkin() { PrimaryColor = 0x6B3410, SecondaryColor = 0xC97B3F, BackgroundColor = 0xF2C890 };
            var tier17 = new ReSkin() { PrimaryColor = 0x3B2352, SecondaryColor = 0x7A4CA8, BackgroundColor = 0xC8A8E0 };
            var tier22 = new ReSkin() { PrimaryColor = 0x5C0E1A, SecondaryColor = 0xA82B3E, BackgroundColor = 0xE8A1A8 };
            var tier27 = new ReSkin() { PrimaryColor = 0x7A5A10, SecondaryColor = 0xD4A017, BackgroundColor = 0xF5DC8C };
        }
    }
}


/*
var posts = new List<Post>();
            posts.AddRange(observations);
            posts.AddRange(comments);
            posts.AddRange(proposals);
            foreach (var post in posts) post.Author.Posts.Add(post);
            foreach (var c in comments) c.Observation.Comments.Add(c);
            foreach (var p in proposals) p.Observation.Proposals.Add(p);

            database.taxons.AddRange(taxons);
            database.users.AddRange(authors);
            database.readings.AddRange(observations);
            database.comments.AddRange(comments);
            database.proposals.AddRange(proposals);



            database.SaveChanges();

*/