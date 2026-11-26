namespace Bison.Core.models.BattlePass;

public record ReskinDTO(
    int ItemId,
    int ItemTier,
    bool IsPaid,
    uint PrimaryColor,
    uint SecondaryColor,
    uint BackgroundColor,
    uint TextColor
) : BattlePassItemDTO(ItemId, ItemTier, IsPaid);