using Bison.Core.models;


namespace Bison.Core.Repositories;


public interface IObservationRepository
{
    Task<IReadOnlyList<Observation>> GetObservations();
    Task<Observation?> GetObservation(int id);
    Task<Observation> AddObservation(Observation observation);
    Task<IReadOnlyList<Observation>> AddObservations(IEnumerable<Observation> observations);
    Task<IReadOnlyList<Observation>> GetTimelineObservations(int? userId, int page = 1);
}