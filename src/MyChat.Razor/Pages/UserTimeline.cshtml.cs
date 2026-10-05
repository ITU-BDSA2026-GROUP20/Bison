using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyChat.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService service;
    public List<ObservationViewModel> Observations { get; set; }
    public int UserId { get; private set; }
    public string Username { get; private set; } = "";
    public int PageNum {get; private set;}

    public UserTimelineModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(int userId, [FromQuery] int? page)                                                                                                                                           
  {                                                                                                                                                                                                      
      if (page is null or < 1)                                                                                                                                                                           
          return Redirect($"/obs/{userId}?page=1");                                                                                                                                                      
                                                                                                                                                                                                         
      Observations = service.GetObservations(userId, page);                                                                                                                                              
      UserId = userId;                                                                                                                                                                                   
      Username = Observations.FirstOrDefault()?.Username ?? $"User {userId}";                                                                                                                            
      PageNum = page.Value;                                                                                                                                                                              
                                                                                                                                                                                                         
      return Page();                                                                                                                                                                                     
  }
}

