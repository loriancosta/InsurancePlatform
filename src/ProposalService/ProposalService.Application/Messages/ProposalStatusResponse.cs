using System.Runtime.Serialization;

namespace ProposalService.Application.Messages;

public record ProposalStatusResponse(
    Guid ProposalId,
    string CustomerName,
    string InsuranceType,
    decimal Value,
    ProposalStatus Status,
    string StatusDescription,
    DateTime CreatedDate,
    DateTime? UpdatedDate = null);

public enum ProposalStatus
{
    [EnumMember(Value = "UnderAnalysis")] UnderAnalysis = 1,
    [EnumMember(Value = "Approved")] Approved = 2,
    [EnumMember(Value = "Rejected")] Rejected = 3
}
