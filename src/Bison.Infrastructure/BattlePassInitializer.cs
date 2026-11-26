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
            var tier2 = new ReSkin() {PrimaryColor = 0x2E5266 };
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