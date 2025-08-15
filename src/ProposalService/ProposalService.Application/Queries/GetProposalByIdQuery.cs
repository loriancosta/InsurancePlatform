using MediatR;
using ProposalService.Application.DTOs;

namespace ProposalService.Application.Queries;

public record GetProposalByIdQuery(Guid ProposalId) : IRequest<ProposalResponse?>;
