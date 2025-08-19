using Azure;
using ContractingService.Domain.Interfaces;
using MassTransit;
using ProposalService.Application.Messages;

namespace ContractingService.Infrastructure.Clients;

public class ProposalServiceMessageClient : IProposalServiceClient
{
    private readonly IRequestClient<ProposalStatusRequest> _requestClient;

    public ProposalServiceMessageClient(IRequestClient<ProposalStatusRequest> requestClient) =>
        _requestClient = requestClient;

    public async Task<ProposalDto?> GetProposalAsync(Guid proposalId)
    {
        try
        {
            // ***************************************************************************
            // **** Vai retornar erro sempre pois não tenho fila de mensagens configurada
            // ***************************************************************************
            var request = new ProposalStatusRequest(proposalId);
            var response = await _requestClient.GetResponse<ProposalStatusResponse>(request);

            if (response.Message is null)
                return null;

            return new ProposalDto(
                response.Message.ProposalId,
                response.Message.CustomerName,
                response.Message.InsuranceType,
                response.Message.Value,
                (int)response.Message.Status,
                response.Message.CreatedDate,
                response.Message.UpdatedDate);
        }
        catch (Exception e)
        {
            // ***************************************************************************
            // **** Vai retornar erro sempre pois não tenho fila de mensagens configurada
            // **** Vou gerar um dado "fake" no errro para simular um retorno real.
            // ***************************************************************************
            return new ProposalDto(
                proposalId,
                "João Silva (Mock)",
                "Seguro Auto",
                1500.00m,
                2, // Status Aprovado
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow);        }
    }
}
