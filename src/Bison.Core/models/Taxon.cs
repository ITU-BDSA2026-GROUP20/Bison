namespace Bison.Core.models;

public class Taxon
{
    public int TaxonId { get; set; }
    public string? VernacularName { get; set; }
    public string? dwc_TaxonID { get; set; }
    public int? ParentTaxonId { get; set; }
    public Taxon? Parent { get; set; }
    public List<Taxon> Children { get; set; } = new();
    public bool IsSubTaxonOf(Taxon other)
    {
        var current = this;
        while(current != null)
        {
            if(current == other) return true;
            current = current.Parent;
        }
        return false;
    }
    //function {:extern} IsSubtaxon(other: Taxon): bool
    
}