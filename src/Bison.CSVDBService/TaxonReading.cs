using CsvHelper.Configuration.Attributes;

namespace Bison.CSVDBService;

internal sealed class TaxonReading
{
    [Name("dwc:taxonID")]
    public string TaxonID {get ; set; } = "";

    [Name("dwc:parentNameUsageID")]
    public string? ParentNameUsageID {get ; set; }

    [Name("dwc:taxonRank")]
    public string TaxonRank {get ; set; } = "";

    [Name("dwc:scientificName")]
    public string ScientificName {get ; set; } = "";

    [Name("dwc:vernacularName")]
    public string? VernacularName {get ; set; }

}