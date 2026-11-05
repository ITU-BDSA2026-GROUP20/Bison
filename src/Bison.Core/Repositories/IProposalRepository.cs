using Bison.Core.models;

namespace Bison.Core.Repositories;

public interface IProposalRepository
{
    Task<IReadOnlyList<Proposal>> GetProposals(int? id);
    Task AddProposal(Proposal proposal);
}