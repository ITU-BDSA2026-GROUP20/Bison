using Bison.Core.models;
using MyChat.Razor;

public class ObservationService : IObservationService
{
    public List<Observation> GetObservationEntities()
    {
        return Client.GetAsync<List<Observation>>("/observationEntitiy").GetAwaiter().GetResult() ?? new();
    }
    public List<ObservationDTO> GetObservations(int? page = null)
    {
        return Client.GetAsync<List<ObservationDTO>>("/obs", ("page", page ?? 1)).GetAwaiter().GetResult() ?? new();
    }

    public List<ObservationDTO> GetObservations(int userId, int? page = null)
    {
        return Client.GetAsync<List<ObservationDTO>>("/obs", ("userId", userId), ("page", page ?? 1)).GetAwaiter().GetResult() ?? new();
    }

    public ObservationDTO? GetObservation(int? id)
    {
        return Client.GetAsync<ObservationDTO>("/observations", ("id", id)).GetAwaiter().GetResult();
    }
    public List<CommentDTO> GetComments(int id)
    {
        return Client.GetAsync<List<Comment>>("/comments", ("id", id)).GetAwaiter().GetResult()?
        .Select(c => new CommentDTO(
            c.Text,
            c.TimeStamp.ToString("yyyy-MM-dd")))
        .ToList()
        ?? new();
    }

    public List<ProposalDTO> GetProposals(int id)
    {
        return Client.GetAsync<List<Proposal>>("/proposals", ("id", id)).GetAwaiter().GetResult()?
        .Select(p => new ProposalDTO(
            p.Taxon?.VernacularName,
            p.TimeStamp.ToString("yyyy-MM-dd")))
        .ToList()
        ?? new();
    }
    public List<TaxonDTO> GetUniqueTaxons()
    {
        return Client.GetAsync<List<Taxon>>("/taxons").GetAwaiter().GetResult()?
        .Where(t => t.VernacularName != null)
        .OrderBy(t => t.VernacularName)
        .Select(t => new TaxonDTO(
            t.TaxonId,
            t?.ParentTaxonId,
            t?.VernacularName))
        .ToList()
        ?? new();
    }
    public Taxon GetTaxon(int id)
    {
        return Client.GetAsync<Taxon>("/taxon", ("id", id)).GetAwaiter().GetResult();
    }
}
