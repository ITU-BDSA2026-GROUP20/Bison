public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int? page = null);
    public List<ObservationViewModel> GetObservations(int userId, int? page = null);
}