using MediatR;
using ContractingService.Application.DTOs;
using ContractingService.Application.Queries;
using ContractingService.Domain.Interfaces;

namespace ContractingService.Application.Handlers;

public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, IEnumerable<ContractResponse>>
{
    private readonly IContractRepository _contractRepository;

    public GetAllContractsQueryHandler(IContractRepository contractRepository) =>
        _contractRepository = contractRepository;

    public async Task<IEnumerable<ContractResponse>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
    {
        var contracts = await _contractRepository.GetAllAsync();
        
        return contracts.Select(contract => new ContractResponse(
            contract.Id,
            contract.ProposalId,
            contract.CustomerName,
            contract.InsuranceType,
            contract.Value,
            contract.ContractDate));
    }
}
