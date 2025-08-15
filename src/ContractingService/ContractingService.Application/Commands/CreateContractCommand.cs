using MediatR;
using ContractingService.Application.DTOs;

namespace ContractingService.Application.Commands;

public record CreateContractCommand(Guid ProposalId) : IRequest<ContractResponse>;
