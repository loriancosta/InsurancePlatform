using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Infrastructure.Repositories;

public class InMemoryProposalRepository : IProposalRepository
{
    private readonly List<Proposal> _proposals = [];

    public InMemoryProposalRepository()
    {
        SeedData();
    }

    public Task<Proposal> CreateAsync(Proposal proposal)
    {
        _proposals.Add(proposal);
        return Task.FromResult(proposal);
    }

    public Task<Proposal?> GetByIdAsync(Guid id)
    {
        var proposal = _proposals.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(proposal);
    }

    public Task<IEnumerable<Proposal>> GetAllAsync()
    {
        var proposals = _proposals.OrderByDescending(p => p.CreatedDate).AsEnumerable();
        return Task.FromResult(proposals);
    }

    public Task<Proposal> UpdateAsync(Proposal proposal)
    {
        var index = _proposals.FindIndex(p => p.Id == proposal.Id);
        if (index >= 0)
        {
            _proposals[index] = proposal;
        }
        return Task.FromResult(proposal);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        var exists = _proposals.Any(p => p.Id == id);
        return Task.FromResult(exists);
    }

    private void SeedData()
    {
        var baseDate = DateTime.UtcNow.AddDays(-30);

        _proposals.AddRange([
            new Proposal(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "João Silva",
                "Seguro Auto",
                1500.00m,
                ProposalStatus.UnderAnalysis,
                baseDate.AddDays(1)),

            new Proposal(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Maria Santos",
                "Seguro Residencial",
                2500.00m,
                ProposalStatus.Approved,
                baseDate.AddDays(5),
                baseDate.AddDays(7)),

            new Proposal(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "Pedro Oliveira",
                "Seguro Vida",
                800.00m,
                ProposalStatus.Rejected,
                baseDate.AddDays(10),
                baseDate.AddDays(12)),

            new Proposal(
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                "Ana Costa",
                "Seguro Auto",
                1800.00m,
                ProposalStatus.Approved,
                baseDate.AddDays(15),
                baseDate.AddDays(16)),

            new Proposal(
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                "Carlos Ferreira",
                "Seguro Empresarial",
                5000.00m,
                ProposalStatus.UnderAnalysis,
                baseDate.AddDays(20))
        ]);
    }
}
