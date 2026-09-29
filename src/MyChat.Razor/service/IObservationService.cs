public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int? page = null);
    public List<ObservationViewModel> GetObservations(string author, int? page = null);
}