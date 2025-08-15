using System.Runtime.Serialization;

namespace ProposalService.Domain.Enums;

public enum ProposalStatus
{
    [EnumMember(Value = "UnderAnalysis")] UnderAnalysis = 1,
    [EnumMember(Value = "Approved")] Approved = 2,
    [EnumMember(Value = "Rejected")] Rejected = 3
}
