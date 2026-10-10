using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;

namespace MyChat.Razor.Pages;

public class AuthorTimelineModel : PageModel
{
    private readonly IObservationService service;
    required public List<ObservationDTO> Observations { get; set; }
    public int AuthorId { get; private set; }
    public string AuthorName { get; private set; } = "";
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
      AuthorName = Observations.FirstOrDefault()?.AuthorName ?? $"Author {userId}";                                                                                                                            
      PageNum = page.Value;                                                                                                                                                                              
                                                                                                                                                                                                         
      return Page();                                                                                                                                                                                     
  }
}

