namespace Bison.Core.models;
using System.Text.Json.Serialization;
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int BattlePassTier { get; set; } = 0;
    public bool BattlePassUpgraded { get; set; } = false;
    public int CurrentXP { get; set; } = 0;
    [JsonIgnore] // Is needed so public timeline filter doesn't try to set Posts
    public List<Post> Posts { get; set; } = new();

    public void AddXp(int amount)
    {
        const int xpPerTier = 100;

        CurrentXP += amount;
        BattlePassTier += CurrentXP / xpPerTier;
        CurrentXP %= xpPerTier;
    }
}