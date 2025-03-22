namespace P2Project.Core.Dtos.Discussions;

public class DiscussionDto
{
    public Guid Id { get; init; }
    public Guid RequestId { get; init; }
    public IEnumerable<MessageDto> Messages { get; init; } = default!;
    public Guid ReviewingUserId { get; init; }
    public Guid ApplicantUserId { get; init; }
    public string Status { get; init; } = string.Empty;
}