
using Bison.Core.models;
using Microsoft.EntityFrameworkCore;
using Bison.Core.Repositories;

namespace Bison.Infrastructure.Repositories;

public class ProposalRepository : IProposalRepository
{
    private readonly Database database; 
    public ProposalRepository(Database database)
    {
        this.database = database;
    }
    
    public async Task<IReadOnlyList<Proposal>> GetProposals(int? observationId)
    {
        if(observationId is null)
        return await database.proposals.ToListAsync();

        return await database.proposals.Where(p=> p.ObservationId == observationId).ToListAsync();
    }
    
    public async Task AddProposal(Proposal proposal)
    {
        database.proposals.Add(proposal);
        await database.SaveChangesAsync();
    }

    public async Task AddProposals(IEnumerable<Proposal> proposals)
    {
        database.proposals.AddRange(proposals);
        await database.SaveChangesAsync();
    }
}