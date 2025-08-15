namespace ProposalService.Application.DTOs;

public record CreateProposalRequest(
    string CustomerName,
    string InsuranceType,
    decimal Value);
