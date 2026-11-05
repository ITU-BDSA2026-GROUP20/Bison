using Bison.Core.models;

namespace Bison.Core.Repositories;

public interface ITaxonRepository
{
    Task<IReadOnlyList<Taxon>> GetTaxons();
    Task<Taxon?> GetTaxon(int id);
}