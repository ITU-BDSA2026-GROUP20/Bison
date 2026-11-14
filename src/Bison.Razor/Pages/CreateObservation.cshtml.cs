using System.ComponentModel.DataAnnotations;
using Bison.Core.models;
using Bison.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

[Authorize]
public class CreateObservationModel : PageModel
{
    private readonly IObservationService service;
    private readonly UserManager<ApplicationUser> userManager;

    public CreateObservationModel(IObservationService service, UserManager<ApplicationUser> userManager)
    {
        this.service = service;
        this.userManager = userManager;
    }

    [BindProperty, Required, StringLength(500)]
    public string Text { get; set; } = string.Empty;

    [BindProperty]
    public int? TaxonId { get; set; }

    public List<TaxonDTO> Taxons { get; private set; } = new();

    public void OnGet() => LoadTaxons();

    public async Task<IActionResult> OnPostAsync()
    {
        LoadTaxons();
        if (!ModelState.IsValid)
            return Page();

        var user = await userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        // Lazy author creation: the first time a user posts, the server creates (or finds) their
        // Author by email, and we remember its id on the user for next time.
        var dto = new CreateObservationDTO(
            Text,
            TaxonId,
            user.AuthorId,
            user.UserName ?? user.Email ?? user.Id,
            user.Email ?? user.UserName ?? user.Id);

        ObservationDTO? created;
        try
        {
            created = await service.AddObservation(dto);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "Could not save the observation. Please try again.");
            return Page();
        }

        if (created is null)
        {
            ModelState.AddModelError(string.Empty, "Could not save the observation. Please try again.");
            return Page();
        }

        if (user.AuthorId != created.AuthorId)
        {
            user.AuthorId = created.AuthorId;
            await userManager.UpdateAsync(user);
        }

        return Redirect($"/ob/{created.Id}");
    }

    private void LoadTaxons() => Taxons = service.GetUniqueTaxons();
}
