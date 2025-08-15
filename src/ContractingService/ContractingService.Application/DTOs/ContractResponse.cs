namespace ContractingService.Application.DTOs;

public record ContractResponse(
    Guid Id,
    Guid ProposalId,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    DateTime ContractDate);
