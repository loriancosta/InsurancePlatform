using MediatR;
using ProposalService.Application.DTOs;
using ProposalService.Domain.Enums;

namespace ProposalService.Application.Commands;

public record UpdateProposalStatusCommand(
    Guid ProposalId,
    ProposalStatus Status) : IRequest<ProposalResponse>;
