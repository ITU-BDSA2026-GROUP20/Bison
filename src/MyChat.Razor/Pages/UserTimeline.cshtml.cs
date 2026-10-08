using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyChat.Razor.Pages;

public class AuthorTimelineModel : PageModel
{
    private readonly IObservationService service;
    public List<ObservationViewModel> Observations { get; set; }
    public int AuthorId { get; private set; }
    public string Authorname { get; private set; } = "";
    public int PageNum {get; private set;}

    public AuthorTimelineModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(int userId, [FromQuery] int? page)                                                                                                                                           
  {                                                                                                                                                                                                      
      if (page is null or < 1)                                                                                                                                                                           
          return Redirect($"/obs/{userId}?page=1");                                                                                                                                                      
                                                                                                                                                                                                         
      Observations = service.GetObservations(userId, page);                                                                                                                                              
      AuthorId = userId;                                                                                                                                                                                   
      Authorname = Observations.FirstOrDefault()?.Authorname ?? $"Author {userId}";                                                                                                                            
      PageNum = page.Value;                                                                                                                                                                              
                                                                                                                                                                                                         
      return Page();                                                                                                                                                                                     
  }
}

