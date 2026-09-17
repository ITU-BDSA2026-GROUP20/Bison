namespace Bison.Core.models;

public class Taxon
{
    public string TaxonId {get; set; }
    public Taxon? Parent {get; private set; }
    public string TaxonRank {get; set; }
    public string ScientificName {get; set; }
    public string? VernacularName {get; set; }

  

    private readonly List<Taxon> _children = new ();
    public IReadOnlyList<Taxon> Children => _children;


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
        parent._children.Add(this);
    }
 
    public override string ToString() => $"{ScientificName} ({TaxonRank})";


}