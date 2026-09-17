using Bison.Core.models;
using CsvHelper;
using System.Reflection;
using System.Globalization;

namespace Bison.CSVDBService;


public sealed class TaxonRepository
{
    private readonly Dictionary<string, Taxon> byID = new();
    private readonly Dictionary<string, Taxon> byVernacularName = new();


    public TaxonRepository(string resourceName, Assembly? assembly = null)
    {
        Load(resourceName, assembly ?? Assembly.GetExecutingAssembly());
    }

    public Taxon? GetByID(string taxonId) => byID.TryGetValue(taxonId, out var taxon) ? taxon : null;

    public Taxon? GetByVernacularName(string vernacularName) => byVernacularName.TryGetValue(vernacularName, out var taxon) ? taxon : null;

    public IReadOnlyCollection<Taxon> All => byID.Values;


    private void Load(string resourceName, Assembly assembly)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found. " + $"Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<TaxonReading>().ToList();
        var parentIdByTaxonId = new Dictionary<string, string?>();

        foreach (var row in records)
        {
            var taxon = new Taxon(row.TaxonID, row.TaxonRank, row.ScientificName, row.VernacularName);

            byID[row.TaxonID] = taxon;
            parentIdByTaxonId[row.TaxonID] = string.IsNullOrEmpty(row.ParentNameUsageID) ? null : row.ParentNameUsageID;

            if (!string.IsNullOrEmpty(row.VernacularName))
                byVernacularName[row.VernacularName] = taxon;
        }

        foreach (var (taxonId, ParentNameUsageId) in parentIdByTaxonId)
        {
            if (ParentNameUsageId != null && byID.TryGetValue(ParentNameUsageId, out var parentTaxon))
            {
                byID[taxonId].SetParent(parentTaxon);
            }
        }
    }

}