using P2Project.Core.Dtos.Common;

namespace P2Project.Core.Dtos.Accounts;

public class VolunteerAccountDto
{
    public Guid VolunteerAccountId { get; init; }
    public Guid UserId { get; init; }
    public int Experience { get; init; } = default!;
    public IEnumerable<AssistanceDetailDto> AssistanceDetails { get; set; } = default!;
    public IEnumerable<CertificateDto> Certificates { get; set; } = default!;
}