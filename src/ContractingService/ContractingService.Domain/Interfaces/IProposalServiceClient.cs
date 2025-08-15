namespace ContractingService.Domain.Interfaces;

public interface IProposalServiceClient
{
    Task<ProposalDto?> GetProposalAsync(Guid proposalId);
}

public record ProposalDto(
    Guid Id,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    int Status,
    DateTime CreatedDate,
    DateTime? UpdatedDate = null);
