
using Bison.Core.models;

public interface IPostRepository
{
    Task<List<Observation>> GetObservations();
    Task<Observation?> GetObservation(int id);
    Task<List<Comment>> GetComments(int? ObservationId);
    Task<List<Proposal>> GetProposals(int? ObservationId);
    Task<List<Observation>> GetTimelineObservations(int? authorId, int page);
    Task AddObservation(Observation observation);
    Task AddComment(Comment comment);
    Task AddProposal(Proposal proposal); 
}