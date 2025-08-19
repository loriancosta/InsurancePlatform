using ProposalService.Domain.Entities;

namespace ProposalService.Domain.Interfaces;

public interface IProposalRepository
{
    Task<Proposal> CreateAsync(Proposal proposal);
    Task<Proposal?> GetByIdAsync(Guid id);
    Task<IEnumerable<Proposal>> GetAllAsync();
    Task<Proposal> UpdateAsync(Proposal proposal);
    Task<bool> ExistsAsync(Guid id);
}
