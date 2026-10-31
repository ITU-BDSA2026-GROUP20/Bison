using Bison.Core.models;
public interface IObservationService
{
    public List<Observation> GetObservationEntities();
    public List<ObservationDTO> GetObservations(int? page = null);
    public List<ObservationDTO> GetObservations(int userId, int? page = null);
    public ObservationDTO? GetObservation(int? id = null);
    public List<CommentDTO> GetComments(int id);
    public List<ProposalDTO> GetProposals(int id);
    public List<TaxonDTO> GetUniqueTaxons();
    public Taxon GetTaxon(int id);
}