
using Bison.Core.models.BattlePass;
using Microsoft.EntityFrameworkCore;
using Bison.Core.Repositories;

namespace Bison.Infrastructure.Repositories;

public class BattlePassRepository : IBattlePassRepository
{
    private readonly Database database; 
    public BattlePassRepository(Database database)
    {
        this.database = database;
    }

    public async Task<BattlePassItem?> getBattlePassItem(int tier, bool isPaid)
    {
        return await database.battlePass
        .AsNoTracking()
        .FirstOrDefaultAsync(i => i.Tier == tier && i.IsPaid == isPaid);

    }
    public async Task<IReadOnlyList<BattlePassItem>> getBattlePassItems()
    {
        return await database.battlePass.ToListAsync();
    }
}