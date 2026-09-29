using MyChat.Razor;

public record ObservationViewModel(string Author, string Observation, string Timestamp);

public class ObservationService : IObservationService
{
    public List<ObservationViewModel> GetObservations(int? page = null)
    {
        List<ObservationViewModel>? observations = Client.GetAsync<List<ObservationViewModel>>("/obs", ("page", page ?? 1)).GetAwaiter().GetResult();
        return observations ?? new List<ObservationViewModel>();
    }

    public List<ObservationViewModel> GetObservations(string author, int? page = null)
    {
        List<ObservationViewModel>? observations = Client.GetAsync<List<ObservationViewModel>>("/obs", ("author", author), ("page", page ?? 1)).GetAwaiter().GetResult();
        return observations ?? new List<ObservationViewModel>();
    }
}
