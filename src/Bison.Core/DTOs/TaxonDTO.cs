namespace Bison.Core.models;

public class Taxon
{
    public string TaxonId { get; set; } = string.Empty;
    public string? VernacularName { get; set; }
    public string? ParentTaxonId { get; set; }
    public Taxon? Parent { get; set; }
    public List<Taxon> Children { get; set; } = new();
}