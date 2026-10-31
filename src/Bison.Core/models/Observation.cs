namespace Bison.Core.models;
public class Observation : Post
{
    public int? TaxonId { get; set; }
    public Taxon? Taxon { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();

    public Taxon GetTaxon() => Taxon ?? throw new InvalidOperationException("Observation has no Taxon loaded.");
    //function {:extern} {:axiom} GetTaxon(): Taxon
}