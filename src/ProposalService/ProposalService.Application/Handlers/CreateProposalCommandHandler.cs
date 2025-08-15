using MediatR;
using ProposalService.Application.Commands;
using ProposalService.Application.DTOs;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Application.Handlers;

public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, ProposalResponse>
{
    private readonly IProposalRepository _proposalRepository;

    public CreateProposalCommandHandler(IProposalRepository proposalRepository) =>
        _proposalRepository = proposalRepository;

    public async Task<ProposalResponse> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        var proposal = Proposal.Create(request.CustomerName, request.InsuranceType, request.Value);
        var createdProposal = await _proposalRepository.CreateAsync(proposal);

        return ProposalResponse.FromProposal(createdProposal);
    }
}
