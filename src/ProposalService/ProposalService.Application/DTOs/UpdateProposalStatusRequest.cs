using ProposalService.Domain.Enums;

namespace ProposalService.Application.DTOs;

public record UpdateProposalStatusRequest(
    Guid ProposalId,
    ProposalStatus Status);
