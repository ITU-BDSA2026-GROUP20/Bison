namespace Bison.Core.models.BattlePass;

public abstract class BattlePassItem
{
    public int Id { get; set; }
    public int Tier { get; set; }
    public bool IsPaid { get; set; }
}
