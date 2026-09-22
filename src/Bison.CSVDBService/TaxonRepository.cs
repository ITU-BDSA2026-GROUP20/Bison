using Bison.Core.models;
using CsvHelper;
using System.Reflection;
using System.Globalization;
using SimpleDB;

namespace Bison.CSVDBService;


public sealed class TaxonRepository
{
    private readonly Dictionary<string, Taxon> byId = new();
    private readonly Dictionary<string, Taxon> byVernacularName = new();

    public TaxonRepository(Assembly? assembly = null)
    {
        Load(assembly ?? Assembly.GetExecutingAssembly());
    }

    public Taxon? GetByID(string taxonId) => byId.TryGetValue(taxonId, out var taxon) ? taxon : null;

    public Taxon? GetByVernacularName(string vernacularName) => byVernacularName.TryGetValue(vernacularName, out var taxon) ? taxon : null;

    public IReadOnlyCollection<Taxon> All => byId.Values;

    private const string ResourceName = "TaxonCsvDatabase.csv";
    private void Load(Assembly assembly)
    {
        using var stream = assembly.GetManifestResourceStream(ResourceName)
        ?? throw new FileNotFoundException($"Embedded resource '{ResourceName}' not found.");
        var repository = new IDatabaseRepositoryImpl<Taxon>(stream);
        var taxons = repository.Read().ToList();

        


        foreach (var taxon in taxons)
        {

            byId[taxon.TaxonId] = taxon;

            if (!string.IsNullOrEmpty(taxon.VernacularName))
                byVernacularName[taxon.VernacularName] = taxon;
        }

        foreach (var taxon in taxons)
        {
            if (!string.IsNullOrEmpty(taxon.ParentTaxonId) && byId.TryGetValue(taxon.ParentTaxonId, out var parentTaxon))
            {
                taxon.SetParent(parentTaxon);
        
            }
        }
    }
}