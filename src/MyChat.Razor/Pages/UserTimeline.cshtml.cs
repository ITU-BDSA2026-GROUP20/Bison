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

    public ActionResult OnGet(string author, int? pageNum)
    {
        Observations = service.GetObservationsFromAuthor(author);

        if(pageNum is null or < 1)
            return RedirectToPage(new {author, pageNum=1});

        Author = author;
        PageNum = pageNum.Value;
        
        return Page();
    }
}

