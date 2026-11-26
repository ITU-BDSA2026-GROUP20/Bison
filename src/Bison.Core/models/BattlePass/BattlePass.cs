namespace Bison.Core.models.BattlePass;

public class BattlePass
{
    public int tierCount { get; set; }
    public Dictionary<int, BattlePassItem>? paidTier { get; set; }
    public Dictionary<int, BattlePassItem>? freeTier { get; set; }
}