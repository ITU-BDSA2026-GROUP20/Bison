using Bison.Core.models;
using MyChat.Razor;

public record ObservationViewModel(int AuthorId, string Authorname, string Observation, string Timestamp);

public class ObservationService : IObservationService
{
    public List<ObservationViewModel> GetObservations(int? page = null)
    {
        List<ObservationViewModel>? observations = Client.GetAsync<List<ObservationViewModel>>("/obs", ("page", page ?? 1)).GetAwaiter().GetResult();
        return observations ?? new List<ObservationViewModel>();
    }

    public List<ObservationViewModel> GetObservations(int userId, int? page = null)
    {
        List<ObservationViewModel>? observations = Client.GetAsync<List<ObservationViewModel>>("/obs", ("userId", userId), ("page", page ?? 1)).GetAwaiter().GetResult();
        return observations ?? new List<ObservationViewModel>();
    }

    public ObservationViewModel? GetObservation(Guid? id)
    {
        ObservationViewModel? observation = Client.GetAsync<ObservationViewModel>("/observations", ("guid", id)).GetAwaiter().GetResult();
        return observation ?? null;
    }
    public List<Comment> GetComments(Guid id)
    {
        List<Comment>? comments = Client.GetAsync<List<Comment>>("/comments",("guid", id)).GetAwaiter().GetResult();
        return comments ?? new List<Comment>();
    }

    public List<Proposal> GetProposals(Guid id)
    {
        List<Proposal>? proposals = Client.GetAsync<List<Proposal>>("/proposals",("guid", id)).GetAwaiter().GetResult();
        return proposals ?? new List<Proposal>();
    }
}
