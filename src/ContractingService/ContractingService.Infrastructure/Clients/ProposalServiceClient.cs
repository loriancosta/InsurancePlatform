using System.Text.Json;
using ContractingService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ContractingService.Infrastructure.Clients;

public class ProposalServiceClient : IProposalServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string _proposalServiceBaseUrl;

    public ProposalServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _proposalServiceBaseUrl = configuration["ProposalService:BaseUrl"] ?? 
            throw new ArgumentNullException("ProposalService:BaseUrl configuration is missing");
    }

    public async Task<ProposalDto?> GetProposalAsync(Guid proposalId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_proposalServiceBaseUrl}/api/proposals/{proposalId}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ProposalDto>(content, options);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
