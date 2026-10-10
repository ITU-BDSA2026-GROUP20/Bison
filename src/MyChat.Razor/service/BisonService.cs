using Bison.Core.models;
using MyChat.Razor;

public class ObservationService : IObservationService
{
    public List<ObservationDTO> GetObservations(int? page = null)
    {
        return Client.GetAsync<List<ObservationDTO>>("/obs", ("page", page ?? 1)).GetAwaiter().GetResult()?
        .Select(o => new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.AuthorName,
            o.TimeStamp))
        .ToList()
        ?? new();
    }

    public List<ObservationDTO> GetObservations(int userId, int? page = null)
    {
        return Client.GetAsync<List<ObservationDTO>>("/obs", ("userId", userId), ("page", page ?? 1)).GetAwaiter().GetResult()?
        .Select(o => new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.AuthorName,
            o.TimeStamp))
        .ToList()
        ?? new();
    }

    public ObservationDTO? GetObservation(int? id)
    {
        ObservationDTO? o = Client.GetAsync<ObservationDTO>("/observations", ("id", id)).GetAwaiter().GetResult();

        if (o is null)
        return null;

        return new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.AuthorName,
            o.TimeStamp
        );
    }
    public List<CommentDTO> GetComments(int id)
    {
        return Client.GetAsync<List<Comment>>("/comments", ("id", id)).GetAwaiter().GetResult()?
        .Select(c => new CommentDTO(
            c.Text,
            c.TimeStamp))
        .ToList()
        ?? new();
    }

    public List<ProposalDTO> GetProposals(int id)
    {
        return Client.GetAsync<List<Proposal>>("/proposals", ("id", id)).GetAwaiter().GetResult()?
        .Select(p => new ProposalDTO(
            p.Taxon.VernacularName,
            p.TimeStamp))
        .ToList()
        ?? new();
    }
}
