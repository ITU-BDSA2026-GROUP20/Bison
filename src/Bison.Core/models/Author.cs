namespace Bison.Core.models;
using System.Text.Json.Serialization;
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore] // Is needed so public timeline filter doesn't try to set Posts
    public List<Post> Posts { get; set; } = new();
}