using MediatR;
using ProposalService.Application.DTOs;

namespace ProposalService.Application.Queries;

public record GetAllProposalsQuery() : IRequest<IEnumerable<ProposalResponse>>;
