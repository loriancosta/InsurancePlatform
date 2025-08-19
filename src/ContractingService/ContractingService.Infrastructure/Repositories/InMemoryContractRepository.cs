using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;
using ProposalService.Domain.Entities;

namespace ContractingService.Infrastructure.Repositories;

public class InMemoryContractRepository : IContractRepository
{
    private readonly List<Contract> _contracts = [];

    public InMemoryContractRepository()
    {
        SeedData(); // <-- Insere alguns dados para testes
    }

    public Task<Contract> CreateAsync(Contract contract)
    {
        _contracts.Add(contract);
        return Task.FromResult(contract);
    }

    public Task<Contract?> GetByIdAsync(Guid id)
    {
        var contract = _contracts.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(contract);
    }

    public Task<Contract?> GetByProposalIdAsync(Guid proposalId)
    {
        var contract = _contracts.FirstOrDefault(c => c.ProposalId == proposalId);
        return Task.FromResult(contract);
    }

    public Task<IEnumerable<Contract>> GetAllAsync()
    {
        var contracts = _contracts.OrderByDescending(c => c.ContractDate).AsEnumerable();
        return Task.FromResult(contracts);
    }

    public Task<bool> ExistsByProposalIdAsync(Guid proposalId)
    {
        var exists = _contracts.Exists(c => c.ProposalId == proposalId);
        return Task.FromResult(exists);
    }

    private void SeedData()
    {
        var baseDate = DateTime.UtcNow.AddDays(-25);

        _contracts.AddRange([
            new Contract(
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Guid.Parse("22222222-2222-2222-2222-222222222222"), // Maria Santos - Aprovada
                "Maria Santos",
                "Seguro Residencial",
                2500.00m,
                baseDate.AddDays(2)),

            new Contract(
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Guid.Parse("44444444-4444-4444-4444-444444444444"), // Ana Costa - Aprovada
                "Ana Costa",
                "Seguro Auto",
                1800.00m,
                baseDate.AddDays(5)),

            new Contract(
                Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Guid.Parse("66666666-6666-6666-6666-666666666666"), // Lorian Bressan - Aprovada
                "Lorian Bressan",
                "Seguro Empresarial",
                10000.00m,
                baseDate.AddDays(2))
        ]);
    }
}
