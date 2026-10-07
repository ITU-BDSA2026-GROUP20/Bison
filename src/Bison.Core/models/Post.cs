namespace Bison.Core.models;

public abstract class Post
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public override string ToString() => $"{Author?.Name} @ {TimeStamp} {Text}";
}