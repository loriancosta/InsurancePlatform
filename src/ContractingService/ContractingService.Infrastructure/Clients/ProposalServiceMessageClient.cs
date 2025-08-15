using MassTransit;
using ContractingService.Domain.Interfaces;
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
        catch (Exception)
        {
            return null;
        }
    }
}
