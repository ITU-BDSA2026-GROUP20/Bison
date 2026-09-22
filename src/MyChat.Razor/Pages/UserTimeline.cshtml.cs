using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyChat.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(string author)
    {
        Observations = service.GetObservationsFromAuthor(author);
        return Page();
    }
}
