
using Bison.Core.models;
using Bison.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bison.Infrastructure.Repositories;

public class ObservationRepository : IObservationRepository
{
    private readonly Database database; 
    public ObservationRepository(Database database)
    {
        this.database = database;
    }

    
    public async Task<IReadOnlyList<Observation>> GetObservations()
    {
        await database.taxons.ToListAsync();
        return await database.readings
        .Include(o=> o.Author)
        .Include(o=> o.Taxon)
        .ToListAsync();
    }
    public async Task<Observation?> GetObservation(int id)
    {
        return await database.readings.Include(o => o.Author).FirstOrDefaultAsync(o => o.Id == id);
    }
   
    public async Task<IReadOnlyList<Observation>> GetTimelineObservations(int? userId, int page)
    {
        IQueryable<Observation> query = database.readings
        .Include(o=> o.Author)
        .Include(o=> o.Taxon);
        
        if(userId is not null) query = query.Where(o=> o.AuthorId == userId.Value); 

        return await query.OrderBy(o=> o.TimeStamp).Skip((page-1)*32).Take(32).ToListAsync(); 
    }

    public async Task<Observation> AddObservation(Observation observation)
    {
        database.readings.Add(observation);
        await database.SaveChangesAsync();
        return observation;
    }
    public async Task<IReadOnlyList<Observation>> AddObservations(IEnumerable<Observation> observations)
    {
        database.readings.AddRange(observations);
        await database.SaveChangesAsync();
        return (IReadOnlyList<Observation>)observations;
    }
}