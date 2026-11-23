
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
        IQueryable<Proposal> query = database.proposals.Include(p => p.Author).Include(p => p.Taxon);
        if (observationId is not null)
            query = query.Where(p => p.ObservationId == observationId);

        return await query.OrderBy(p => p.TimeStamp).ToListAsync();
    }

    public async Task<Proposal> AddProposal(Proposal proposal)
    {
        database.proposals.Add(proposal);
        await database.SaveChangesAsync();
        return proposal;
    }

    public async Task AddProposals(IEnumerable<Proposal> proposals)
    {
        database.proposals.AddRange(proposals);
        await database.SaveChangesAsync();
    }
}