using Bison.Core.models;
using MyChat.Razor;

public class ObservationService : IObservationService
{
    public List<ObservationDTO> GetObservations(int? page = null)
    {
        return Client.GetAsync<List<Observation>>("/obs", ("page", page ?? 1)).GetAwaiter().GetResult()?
        .Select(o => new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.Author.Name,
            o.TimeStamp.ToString("yyyy-MM-dd")))
        .ToList()
        ?? new();
    }

    public List<ObservationDTO> GetObservations(int userId, int? page = null)
    {
        return Client.GetAsync<List<Observation>>("/obs", ("userId", userId), ("page", page ?? 1)).GetAwaiter().GetResult()?
        .Select(o => new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.Author.Name,
            o.TimeStamp.ToString("yyyy-MM-dd")))
        .ToList()
        ?? new();
    }

    public ObservationDTO? GetObservation(int? id)
    {
        Observation? o = Client.GetAsync<Observation>("/observations", ("id", id)).GetAwaiter().GetResult();

        if (o is null)
        return null;

        return new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.Author.Name,
            o.TimeStamp.ToString("yyyy-MM-dd")
        );
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
            p.Taxon.VernacularName,
            p.TimeStamp.ToString("yyyy-MM-dd")))
        .ToList()
        ?? new();
    }
}
