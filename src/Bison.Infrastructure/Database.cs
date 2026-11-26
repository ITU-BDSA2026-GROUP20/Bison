using Bison.Core.models;
using Bison.Core.models.BattlePass;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bison.Infrastructure
{
    public sealed class Database : IdentityDbContext<ApplicationUser>
    {
        public Database(DbContextOptions<Database> options) : base(options) {}

        public DbSet<Observation> readings { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Proposal> proposals { get; set; }
        public DbSet<Taxon> taxons { get; set; }
        public DbSet<Author> users { get; set; }
        public DbSet<BattlePassItem> battlePass { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ReSkin>();
            builder.Entity<Unlock>();
            builder.Entity<BattlePassItem>().HasIndex(i => new { i.Tier, i.IsPaid }).IsUnique();
        }
    }
}
