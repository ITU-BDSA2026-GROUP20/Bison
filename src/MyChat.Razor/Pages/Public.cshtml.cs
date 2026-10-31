using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.models;

namespace MyChat.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService service;
    required public List<ObservationDTO> Observations { get; set; }
    public List<TaxonDTO> UniTaxons { get; set; }
    
    [BindProperty(SupportsGet = true, Name = "taxon")]
    public int? TaxonFilter { get; set; }

    public PublicModel(IObservationService ser)
    {
        service = ser;
    }

    public ActionResult OnGet()
    {
        var all = service.GetObservations();
        UniTaxons = service.GetUniqueTaxons();

        if (TaxonFilter is int id)
        {
            var root = service.GetTaxon(TaxonFilter.Value);
            var raw = service.GetObservationEntities();
            
            var entities = service.GetObservationEntities().Where(e => e.Taxon is not null).ToArray();

            var seq = Dafny.Sequence<Observation>.FromArray(entities);
            var kept = Bison.Core.dafny.Taxonomy.__default.FilterByTaxon(root, seq).Elements.Select(o => o.Id).ToHashSet();

            Observations = all.Where(d => kept.Contains(d.Id)).ToList();
        }
        else 
            Observations = all;

        return Page();
    }
}
