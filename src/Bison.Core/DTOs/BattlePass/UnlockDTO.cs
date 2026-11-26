namespace Bison.Core.models.BattlePass;

public record UnlockDTO(
    int ItemId,
    int ItemTier,
    bool IsPaid,
    UnlockType Feature
) : BattlePassItemDTO(ItemId, ItemTier, IsPaid);