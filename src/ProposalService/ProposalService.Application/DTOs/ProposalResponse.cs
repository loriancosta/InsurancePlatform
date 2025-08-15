using ProposalService.Domain.Enums;

namespace ProposalService.Application.DTOs;

public record ProposalResponse(
    Guid Id,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    ProposalStatus Status,
    string StatusDescription,
    DateTime CreatedDate,
    DateTime? UpdatedDate = null)
{
    public static ProposalResponse FromProposal(Domain.Entities.Proposal proposal) =>
        new(
            proposal.Id,
            proposal.CustomerName,
            proposal.InsuranceType,
            proposal.Value,
            proposal.Status,
            GetStatusDescription(proposal.Status),
            proposal.CreatedDate,
            proposal.UpdatedDate);

    private static string GetStatusDescription(ProposalStatus status) => status switch
    {
        ProposalStatus.UnderAnalysis => "Em Análise",
        ProposalStatus.Approved => "Aprovada",
        ProposalStatus.Rejected => "Rejeitada",
        _ => "Status Desconhecido"
    };
}
