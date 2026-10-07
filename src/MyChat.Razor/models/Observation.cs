namespace MyChat.Razor.Models;

public class Observation : Post
{
    public string TaxonId { get; set; } = string.Empty;
    public Taxon Taxon { get; set; } = null!;
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();
}