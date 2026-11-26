using Bison.Core.models.BattlePass;
using Bison.Core.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class BattlePassModel : PageModel
{
    private readonly IObservationService service;

    public BattlePassModel(IObservationService service)
    {
        this.service = service;
    }

    public IReadOnlyList<BattlePassItemDTO> Items { get; private set; } = Array.Empty<BattlePassItemDTO>();

    public async Task OnGetAsync()
    {
        Items = service.GetBattlePassItems();
    }
}