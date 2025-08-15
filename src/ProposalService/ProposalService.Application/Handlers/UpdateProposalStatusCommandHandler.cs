using MediatR;
using ProposalService.Application.Commands;
using ProposalService.Application.DTOs;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Application.Handlers;

public class UpdateProposalStatusCommandHandler : IRequestHandler<UpdateProposalStatusCommand, ProposalResponse>
{
    private readonly IProposalRepository _proposalRepository;

    public UpdateProposalStatusCommandHandler(IProposalRepository proposalRepository) =>
        _proposalRepository = proposalRepository;

    public async Task<ProposalResponse> Handle(UpdateProposalStatusCommand request, CancellationToken cancellationToken)
    {
        var proposal = await _proposalRepository.GetByIdAsync(request.ProposalId);
        if (proposal is null)
            throw new ArgumentException($"Proposal with ID {request.ProposalId} not found.");

        var updatedProposal = proposal.UpdateStatus(request.Status);
        var savedProposal = await _proposalRepository.UpdateAsync(updatedProposal);

        return ProposalResponse.FromProposal(savedProposal);
    }
}
