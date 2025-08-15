using MassTransit;
using ProposalService.Application.Messages;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Infrastructure.Consumers;

public class ProposalStatusRequestConsumer : IConsumer<ProposalStatusRequest>
{
    private readonly IProposalRepository _proposalRepository;

    public ProposalStatusRequestConsumer(IProposalRepository proposalRepository) =>
        _proposalRepository = proposalRepository;

    public async Task Consume(ConsumeContext<ProposalStatusRequest> context)
    {
        var proposal = await _proposalRepository.GetByIdAsync(context.Message.ProposalId);
        
        if (proposal is null)
        {
            await context.RespondAsync(new ProposalStatusResponse(
                context.Message.ProposalId,
                string.Empty,
                string.Empty,
                0,
                ProposalStatus.UnderAnalysis,
                "Em Análise",
                DateTime.MinValue,
                null));
            return;
        }

        var response = new ProposalStatusResponse(
            proposal.Id,
            proposal.CustomerName,
            proposal.InsuranceType,
            proposal.Value,
            (ProposalService.Application.Messages.ProposalStatus)proposal.Status,
            GetStatusDescription((ProposalService.Application.Messages.ProposalStatus)proposal.Status),
            proposal.CreatedDate,
            proposal.UpdatedDate);

        await context.RespondAsync(response);
    }

    private static string GetStatusDescription(ProposalStatus status) => status switch
    {
        ProposalStatus.UnderAnalysis => "Em Análise",
        ProposalStatus.Approved => "Aprovada",
        ProposalStatus.Rejected => "Rejeitada",
        _ => "Status Desconhecido"
    };
}
