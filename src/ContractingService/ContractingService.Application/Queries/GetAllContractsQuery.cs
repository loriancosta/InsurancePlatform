using MediatR;
using ContractingService.Application.DTOs;

namespace ContractingService.Application.Queries;

public record GetAllContractsQuery() : IRequest<IEnumerable<ContractResponse>>;
