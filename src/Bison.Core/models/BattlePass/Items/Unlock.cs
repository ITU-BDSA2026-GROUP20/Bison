namespace Bison.Core.models.BattlePass;

public enum UnlockType { Badge1, Badge2, Badge3, Gif, Banner, Avatar, Emote, Title }

public sealed class Unlock : BattlePassItem
{
    public UnlockType Feature { get; set; }
}
