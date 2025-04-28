using P2Project.Core.Dtos.Common;

namespace P2Project.Core.Dtos.Accounts;

public class VolunteerAccountDto
{
    public Guid VolunteerAccountId { get; set; }
    public Guid UserId { get; set; }
    public int Experience { get; set; } = default!;
    public IEnumerable<AssistanceDetailDto> AssistanceDetails { get; set; } = default!;
    public IEnumerable<CertificateDto> Certificates { get; set; } = default!;
}