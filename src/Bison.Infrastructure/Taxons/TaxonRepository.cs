
using Bison.Core.models;
using Microsoft.EntityFrameworkCore;
using Bison.Core.Repositories;

namespace Bison.Infrastructure.Repositories;

public class TaxonRepository : ITaxonRepository
{
    private readonly Database database; 
    public TaxonRepository(Database database)
    {
        this.database = database;
    }
    public async Task<IReadOnlyList<Taxon>> GetTaxons()
    {
        return await database.taxons.OrderBy(t => t.VernacularName).ToListAsync();
    }
    public async Task<Taxon?> GetTaxon(int id)
    {
        return await database.taxons.FirstOrDefaultAsync(t => t.TaxonId == id);
    }
}