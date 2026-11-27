using Bison.Core.models;
using Microsoft.EntityFrameworkCore;
using Bison.Core.models.BattlePass;

namespace Bison.Infrastructure;

public sealed class BattlePassInitializer(Database database)
{
    public async Task InitializeAsync()
    {
        await database.Database.MigrateAsync();

        if (!database.battlePass.Any())
        {
            SeedDatabase();

            await database.SaveChangesAsync();
        }
    }

    private void SeedDatabase()
    {
        if (!database.battlePass.Any())
        {
            List<BattlePassItem> items = new List<BattlePassItem>()
            {
                // Free
                new Unlock() {Tier = 1, Feature = UnlockType.Gif, IsPaid = false },
                new ReSkin() {Tier = 2, PrimaryColor = 0x7A5A10, SecondaryColor = 0xD4A017, BackgroundColor = 0xF5DC8C, IsPaid = false },
                new ReSkin() {Tier = 10, PrimaryColor = 0x7A5A10, SecondaryColor = 0xD4A017, BackgroundColor = 0xF5DC8C, IsPaid = false },

                // Paid
                new Unlock() {Tier = 1, Feature = UnlockType.Badge1, IsPaid = true },
                new ReSkin() {Tier = 2, PrimaryColor = 0x2E5266, SecondaryColor = 0x6E8898, BackgroundColor =  0x9FB1BC, IsPaid = true},
                new ReSkin() {Tier = 7, PrimaryColor = 0x2D4A2B, SecondaryColor = 0x6B8E4E, BackgroundColor = 0xA9C48C, IsPaid = true},
                new ReSkin() {Tier = 12, PrimaryColor = 0x6B3410, SecondaryColor = 0xC97B3F, BackgroundColor = 0xF2C890, IsPaid = true },
                new Unlock() {Tier = 13, Feature = UnlockType.Badge2, IsPaid = true},
                new ReSkin() {Tier = 17, PrimaryColor = 0x3B2352, SecondaryColor = 0x7A4CA8, BackgroundColor = 0xC8A8E0, IsPaid = true },
                new ReSkin() {Tier = 23, PrimaryColor = 0x5C0E1A, SecondaryColor = 0xA82B3E, BackgroundColor = 0xE8A1A8, IsPaid = true },
                new Unlock() {Tier = 25, Feature = UnlockType.Badge3, IsPaid = true},
            };

            database.battlePass.AddRange(items);
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