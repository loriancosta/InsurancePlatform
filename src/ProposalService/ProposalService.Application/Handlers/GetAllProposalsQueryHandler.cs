using MediatR;
using ProposalService.Application.DTOs;
using ProposalService.Application.Queries;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Application.Handlers;

public class GetAllProposalsQueryHandler : IRequestHandler<GetAllProposalsQuery, IEnumerable<ProposalResponse>>
{
    private readonly IProposalRepository _proposalRepository;

    public GetAllProposalsQueryHandler(IProposalRepository proposalRepository) =>
        _proposalRepository = proposalRepository;

    public async Task<IEnumerable<ProposalResponse>> Handle(GetAllProposalsQuery request, CancellationToken cancellationToken)
    {
        var proposals = await _proposalRepository.GetAllAsync();
        
        return proposals.Select(ProposalResponse.FromProposal);
    }
}
