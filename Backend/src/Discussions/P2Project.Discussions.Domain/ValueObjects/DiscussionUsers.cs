namespace P2Project.Discussions.Domain.ValueObjects;

public class DiscussionUsers
{
    private DiscussionUsers() { }
    public Guid ReviewingUserId { get; set; }
    public Guid ApplicantUserId { get; set; }
    
    private DiscussionUsers(Guid reviewingUserId, Guid applicantUserId)
    {
        ReviewingUserId = reviewingUserId;
        ApplicantUserId = applicantUserId;
    }
    
    public static DiscussionUsers Create(Guid reviewingUserId, Guid applicantUserId) =>
        new (reviewingUserId, applicantUserId);
}