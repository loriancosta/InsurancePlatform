using ProposalService.Domain.Enums;

namespace ProposalService.Domain.Entities;

public record Proposal(
    Guid Id,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    ProposalStatus Status,
    DateTime CreatedDate,
    DateTime? UpdatedDate = null)
{
    public static Proposal Create(string customerName, string insuranceType, decimal value) =>
        new(Guid.NewGuid(), customerName, insuranceType, value, ProposalStatus.UnderAnalysis, DateTime.UtcNow);

    public Proposal UpdateStatus(ProposalStatus newStatus) =>
        this with { Status = newStatus, UpdatedDate = DateTime.UtcNow };
}
