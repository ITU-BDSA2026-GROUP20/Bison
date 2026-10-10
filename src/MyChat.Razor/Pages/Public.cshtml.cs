using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;

namespace MyChat.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService service;
    required public List<ObservationDTO> Observations { get; set; }

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
