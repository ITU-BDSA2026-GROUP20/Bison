using CsvHelper.Configuration.Attributes;

namespace Bison.Core.models;

public class Taxon
{
    [Name("dwc:taxonId")]
    public string TaxonId {get; set; } = "";
    
    [Name("dwc:parentNameUsageId")]
    public string? ParentTaxonId {get; set; }
    public Taxon? Parent {get; private set; }
    
    [Name("dwc:taxonRank")]
    public string TaxonRank {get; set; } = "";

    [Name("dwc:scientificName")]
    public string ScientificName {get; set; } = "";
    
    [Name("dwc:vernacularName")]
    public string? VernacularName {get; set; }
    private readonly List<Taxon> children = new ();
    public IReadOnlyList<Taxon> Children => children;

    public Taxon() { }
    public Taxon(string taxonId, string taxonRank, string scientificName, string? vernacularName = null)
    {
        TaxonId = taxonId;
        TaxonRank = taxonRank;
        ScientificName = scientificName;
        VernacularName = vernacularName;
    }
    public void SetParent(Taxon parent)
    {
        Parent = parent;
        parent.children.Add(this);
    }
    public override string ToString() => $"{ScientificName} ({TaxonRank})";
}