
using Bison.Core.models;
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly Database database; 
    public PostRepository(Database database)
    {
        this.database = database;
    }

    
    public async Task<List<Observation>> GetObservations()
    {
        return await database.readings.ToListAsync();
    }
    public async Task<Observation?> GetObservation(int id)
    {
        return await database.readings.FindAsync(id);
    }
    public async Task<List<Comment>> GetComments(int? observationId)
    {
        if(observationId is null)
        return await database.comments.ToListAsync();

        return await database.comments.Where(c=> c.ObservationId == observationId).ToListAsync();
    }
    public async Task<List<Proposal>> GetProposals(int? observationId)
    {
        if(observationId is null)
        return await database.proposals.ToListAsync();

        return await database.proposals.Where(p=> p.ObservationId == observationId).ToListAsync();
    }
   
    public async Task<List<Observation>> GetTimelineObservations(int? userId, int page)
    {
        IQueryable<Observation> query = database.readings.Include(o=> o.Author);
        
        if(userId is not null) query = query.Where(o=> o.AuthorId == userId.Value); 

        return await query.OrderBy(o=> o.TimeStamp).Skip((page-1)*32).Take(32).ToListAsync(); 
    }

    public async Task AddObservation(Observation observation)
    {
        database.readings.Add(observation);
        await database.SaveChangesAsync(); 
    }
    public async Task AddComment(Comment comment)
    {
        database.comments.Add(comment);
        await database.SaveChangesAsync();
    }
    public async Task AddProposal(Proposal proposal)
    {
        database.proposals.Add(proposal);
        await database.SaveChangesAsync();
    }
}