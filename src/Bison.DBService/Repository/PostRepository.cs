
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
    public async Task<List<Comment>> GetComments(int observationId)
    {
        return await database.comments.Where(c=> c.ObservationId == observationId).ToListAsync();
    }
    public async Task<List<Proposal>> GetProposals(int observationId)
    {
        return await database.proposals.Where(p=> p.ObservationId == observationId).ToListAsync();
    }
   
    public async Task AddObservation(Observation observation)
    {
        database.Observation.Add(Observation);
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