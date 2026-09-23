using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyChat.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService service;
    public List<ObservationViewModel> Observations { get; set; }
    public string Author {get; private set;} = "";
    public int PageNum {get; private set;}

    public UserTimelineModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(string author, [FromQuery] int? page)
    {
        if (page is null or < 1)
            return Redirect($"/obs/{Uri.EscapeDataString(author)}?page=1");

        Observations = service.GetObservations(author, page);
        Author = author;
        PageNum = page.Value;

        return Page();
    }
}

