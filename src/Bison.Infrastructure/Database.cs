using Bison.Core.models;
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
    }
}
