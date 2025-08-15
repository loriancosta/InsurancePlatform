using MediatR;
using ProposalService.Application.DTOs;

namespace ProposalService.Application.Commands;

public record CreateProposalCommand(
    string CustomerName,
    string InsuranceType,
    decimal Value) : IRequest<ProposalResponse>;
