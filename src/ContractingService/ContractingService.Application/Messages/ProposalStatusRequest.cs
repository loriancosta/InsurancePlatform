namespace ContractingService.Application.Messages;

/// <summary>
/// Mensagem para solicitar verificação de status de proposta via mensageria.
/// NOTA: Este é um exemplo de comunicação via mensageria mantido em paralelo 
/// ao HTTP REST para fins de demonstração e comparação de abordagens.
/// </summary>
public record ProposalStatusRequest
{
    public Guid ProposalId { get; init; }
    public Guid RequestId { get; init; } = Guid.NewGuid();
    public DateTime RequestedAt { get; init; } = DateTime.UtcNow;
}
