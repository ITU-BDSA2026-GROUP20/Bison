using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;
using Bison.Infrastructure;

namespace Bison.Razor.Pages;

public class ObservationDetailsModel : PageModel
{
    private readonly IObservationService service;
    private readonly UserManager<ApplicationUser> userManager;
    required public ObservationDTO? Observation { get; set; }
    required public List<ProposalDTO> Proposals { get; set; }
    required public List<CommentDTO> Comments { get; set; }
    public List<TaxonDTO> Taxons { get; private set; } = new();
    public int? Id {get; private set;} = null;

    [BindProperty]
    public string? CommentText { get; set; }

    [BindProperty]
    public string? ProposalText { get; set; }

    [BindProperty]
    public int? ProposalTaxonId { get; set; }

    public ObservationDetailsModel(IObservationService ser, UserManager<ApplicationUser> userManager)
    {
        service = ser;
        this.userManager = userManager;
    }

    public ActionResult OnGet(int? id) => Load(id) ? Page() : Redirect("/");

    public async Task<IActionResult> OnPostCommentAsync(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var created = await service.AddComment(new CreateCommentDTO(id, CommentText ?? string.Empty, user.UserName ?? user.Email ?? user.Id, user.Email ?? user.UserName ?? user.Id, user.AuthorId));
        return await CompletePost(id, user, created?.AuthorId, nameof(CommentText), "Could not save the comment. Please try again.");
    }

    public async Task<IActionResult> OnPostProposalAsync(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var created = await service.AddProposal(new CreateProposalDTO(id, ProposalTaxonId, ProposalText ?? string.Empty, user.UserName ?? user.Email ?? user.Id, user.Email ?? user.UserName ?? user.Id, user.AuthorId));
        return await CompletePost(id, user, created?.AuthorId, nameof(ProposalText), "Could not save the proposal. Please try again.");
    }

    private async Task<IActionResult> CompletePost(int id, ApplicationUser user, int? authorId, string field, string error)
    {
        if (authorId is null)
        {
            ModelState.AddModelError(field, error);
            return Load(id) ? Page() : Redirect("/");
        }

        if (user.AuthorId != authorId)
        {
            user.AuthorId = authorId;
            await userManager.UpdateAsync(user);
        }

        return Redirect($"/ob/{id}");
    }

    private bool Load(int? id)
    {
        Observation = service.GetObservation(id);
        if (Observation is null) return false;

        Id = Observation.Id;
        Proposals = service.GetProposals(Observation.Id);
        Comments = service.GetComments(Observation.Id);
        Taxons = service.GetUniqueTaxons();
        return true;
    }
}
