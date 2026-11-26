namespace Bison.Core.models.BattlePass;

public enum UnlockType { Badge, Gif, Banner, Avatar, Emote, Title }

public sealed class Unlock : BattlePassItem
{
    public UnlockType Feature { get; set; }
}
