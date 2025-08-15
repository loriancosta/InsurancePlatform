using MediatR;
using ContractingService.Application.Commands;
using ContractingService.Application.DTOs;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;

namespace ContractingService.Application.Handlers;

public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, ContractResponse>
{
    private readonly IContractRepository _contractRepository;
    private readonly IProposalServiceClient _proposalServiceClient;

    public CreateContractCommandHandler(IContractRepository contractRepository, IProposalServiceClient proposalServiceClient)
    {
        _contractRepository = contractRepository;
        _proposalServiceClient = proposalServiceClient;
    }

    public async Task<ContractResponse> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        var existingContract = await _contractRepository.ExistsByProposalIdAsync(request.ProposalId);
        if (existingContract)
            throw new InvalidOperationException($"Contract for proposal {request.ProposalId} already exists.");

        var proposal = await _proposalServiceClient.GetProposalAsync(request.ProposalId);
        if (proposal is null)
            throw new ArgumentException($"Proposal with ID {request.ProposalId} not found.");

        if (proposal.Status != 2) // 2 = Approved
            throw new InvalidOperationException($"Proposal {request.ProposalId} is not approved. Current status: {proposal.Status}");

        var contract = Contract.Create(proposal.Id, proposal.CustomerName, proposal.InsuranceType, proposal.Value);
        var createdContract = await _contractRepository.CreateAsync(contract);

        return new ContractResponse(
            createdContract.Id,
            createdContract.ProposalId,
            createdContract.CustomerName,
            createdContract.InsuranceType,
            createdContract.Value,
            createdContract.ContractDate);
    }
}
