using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;
using System.Runtime.CompilerServices;

namespace MyChat.Razor.Pages;

public class ObservationDetailsModel : PageModel
{
    private readonly IObservationService service;
    public ObservationViewModel Observation { get; set; }
    public List<Proposal> Proposals { get; set; }
    public List<Comment> Comments { get; set; }
    public Guid? Id {get; private set;} = null;

    public ObservationDetailsModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(Guid? id)
    {
        if (id == Guid.Empty || id is null)
            return Redirect("/obs");

        Observation = service.GetObservation(id);
        
        // Invalid observation id, redirect to the list of observations
        if (Observation is null)
            return Redirect("/obs");
 
        Id = id;
        Proposals = service.GetProposals(id.Value); 
        Comments =  service.GetComments(id.Value);
        return Page();
    }
}
