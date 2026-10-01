using MyChat.Razor;

public record ObservationViewModel(int UserId, string Username, string Observation, string Timestamp);

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
}
