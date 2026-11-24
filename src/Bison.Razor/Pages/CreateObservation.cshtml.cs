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
    [BindProperty]
    public IFormFile? Photo { get; set; }
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

        // Won't be used if Photo is null
        string? photoName = null; 
        if (Photo is not null)
        {
            // Validate the photo file type
            var ext = Path.GetExtension(Photo.FileName).ToLowerInvariant();
            if (ext is not (".jpg" or ".jpeg" or ".png"))
            {
                ModelState.AddModelError(string.Empty, "Invalid photo format. Only JPG and PNG are allowed.");
                return Page();
            }
            // Generate unique filename for the photo if it exists
            photoName = $"{Guid.NewGuid()}{ext}";

            // Save the photo to storage
            var path = Path.Combine("wwwroot", "uploads", photoName!);
            await using var stream = new FileStream(path, FileMode.Create);
            await Photo!.CopyToAsync(stream);
        }

        // Lazy author creation: the first time a user posts, the server creates (or finds) their
        // Author by email, and we remember its id on the user for next time.
        var dto = new CreateObservationDTO(
            Text,
            user.UserName ?? user.Email ?? user.Id,
            user.Email ?? user.UserName ?? user.Id,
            TaxonId,
            user.AuthorId,
            photoName
        );

        ObservationDTO? created;
        created = await service.AddObservation(dto);

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
