using ContractingService.Domain.Entities;

namespace ContractingService.Domain.Interfaces;

public interface IContractRepository
{
    Task<Contract> CreateAsync(Contract contract);
    Task<Contract?> GetByIdAsync(Guid id);
    Task<Contract?> GetByProposalIdAsync(Guid proposalId);
    Task<IEnumerable<Contract>> GetAllAsync();
    Task<bool> ExistsByProposalIdAsync(Guid proposalId);
}
