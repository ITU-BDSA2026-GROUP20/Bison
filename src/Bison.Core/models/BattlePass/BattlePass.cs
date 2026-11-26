public class BattlePass
{
    
    int tierCount { get; set; }
    Dictionary<int, BattlePassItem> paidTier { get; set; }
    Dictionary<int, BattlePassItem> freeTier { get; set; }
    

}