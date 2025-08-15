using MediatR;
using ProposalService.Application.DTOs;
using ProposalService.Application.Queries;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Application.Handlers;

public class GetProposalByIdQueryHandler : IRequestHandler<GetProposalByIdQuery, ProposalResponse?>
{
    private readonly IProposalRepository _proposalRepository;

    public GetProposalByIdQueryHandler(IProposalRepository proposalRepository) =>
        _proposalRepository = proposalRepository;

    public async Task<ProposalResponse?> Handle(GetProposalByIdQuery request, CancellationToken cancellationToken)
    {
        var proposal = await _proposalRepository.GetByIdAsync(request.ProposalId);
        
        return proposal is null ? null : ProposalResponse.FromProposal(proposal);
    }
}
