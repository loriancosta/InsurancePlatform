namespace ContractingService.Domain.Entities;

public record Contract(
    Guid Id,
    Guid ProposalId,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    DateTime ContractDate)
{
    public static Contract Create(Guid proposalId, string customerName, string insuranceType, decimal value) =>
        new(Guid.NewGuid(), proposalId, customerName, insuranceType, value, DateTime.UtcNow);
}
