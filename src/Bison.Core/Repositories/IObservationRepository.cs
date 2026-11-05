using Bison.Core.models;


namespace Bison.Core.Repositories;


public interface IObservationRepository
{
    Task<IReadOnlyList<ObservationDTO>> GetAll();
    Task<ObservationDTO?> GetById(int id);
    Task<Observation> CreateObservation(Observation observation);
    Task<IReadOnlyList<ObservationDTO>> GetByPage(int? userId, int page = 1);
}