using Bison.Core.models.BattlePass;
using Bison.Core.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class BattlePassModel : PageModel
{
    private readonly IBattlePassRepository repository;

    public BattlePassModel(IBattlePassRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyList<BattlePassItem> Items { get; private set; } = Array.Empty<BattlePassItem>();

    public async Task OnGetAsync()
    {
        Items = await repository.getBattlePassItems();
    }
}