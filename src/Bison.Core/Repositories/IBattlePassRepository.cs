using Bison.Core.models;
using Bison.Core.models.BattlePass;
namespace Bison.Core.Repositories;

public interface IBattlePassRepository
{
    Task<BattlePassItem?> getBattlePassItem(int tier, bool isPaid);
    Task<IReadOnlyList<BattlePassItem>> getBattlePassItems();

}