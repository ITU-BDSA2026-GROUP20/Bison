using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;
using System.Runtime.CompilerServices;

namespace MyChat.Razor.Pages;

public class ObservationDetailsModel : PageModel
{
    private readonly IObservationService service;
    required public ObservationDTO Observation { get; set; }
    required public List<ProposalDTO> Proposals { get; set; }
    required public List<CommentDTO> Comments { get; set; }
    public int? Id {get; private set;} = null;

    public ObservationDetailsModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet(int? id)
    {
        if (id is null || id < 0)
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
