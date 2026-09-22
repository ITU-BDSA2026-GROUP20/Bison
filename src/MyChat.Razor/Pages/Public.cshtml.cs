using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyChat.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService service;
    public List<ObservationViewModel> Observations { get; set; }

    public PublicModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet()
    {
        Observations = service.GetObservations();
        return Page();
    }
}
