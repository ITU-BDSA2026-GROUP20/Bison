using Microsoft.EntityFrameworkCore;
using Bison.Core.models;

public class Database : DbContext
{
    public Database(DbContextOptions<Database> options) : base(options)
    {
        
    }
    public DbSet<Reading> readings { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Proposal> proposals { get; set; }
    public DbSet<Taxon> taxons { get; set; }

}