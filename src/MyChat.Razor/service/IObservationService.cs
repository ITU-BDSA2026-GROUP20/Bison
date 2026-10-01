using Bison.Core.models;
public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int? page = null);
    public List<ObservationViewModel> GetObservations(string author, int? page = null);
    public ObservationViewModel GetObservation(Guid? id = null);
    public List<Comment> GetComments(Guid id);

    public List<Proposal> GetProposals(Guid id);
}