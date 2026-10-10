namespace Bison.Core.models;
public class AuthorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = new();
}