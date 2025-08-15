using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;

namespace ProposalService.Domain.Interfaces;

public interface IProposalRepository
{
    Task<Proposal> CreateAsync(Proposal proposal);
    Task<Proposal?> GetByIdAsync(Guid id);
    Task<IEnumerable<Proposal>> GetAllAsync();
    Task<Proposal> UpdateAsync(Proposal proposal);
    Task<bool> ExistsAsync(Guid id);
}
