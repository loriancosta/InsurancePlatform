namespace ContractingService.Application.Messages;

/// <summary>
/// Resposta com status da proposta via mensageria.
/// NOTA: Este é um exemplo de comunicação via mensageria mantido em paralelo 
/// ao HTTP REST para fins de demonstração e comparação de abordagens.
/// </summary>
public record ProposalStatusResponse
{
    public Guid ProposalId { get; init; }
    public Guid RequestId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string InsuranceType { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public int Status { get; init; } // 1=UnderAnalysis, 2=Approved, 3=Rejected
    public DateTime RespondedAt { get; init; } = DateTime.UtcNow;
    public bool IsApproved => Status == 2;
}
