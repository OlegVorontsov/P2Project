using CSharpFunctionalExtensions;
using P2Project.SharedKernel.BaseClasses;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using P2Project.VolunteerRequests.Domain.Enums;
using P2Project.VolunteerRequests.Domain.ValueObjects;

namespace P2Project.VolunteerRequests.Domain;

public class VolunteerRequest : DomainEntity<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id) : base(id) {}
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public FullName FullName { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    
    private VolunteerRequest(
        VolunteerRequestId requestId,
        Guid userId,
        FullName fullName,
        VolunteerInfo volunteerInfo) : base(requestId)
    {
        Id = requestId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Submitted;
        FullName = fullName;
        VolunteerInfo = volunteerInfo;
    }
    
    public static Result<VolunteerRequest, Error> Create(
        Guid requestId,
        Guid userId,
        FullName fullName,
        VolunteerInfo volunteerInfo)
    {
        var request = new VolunteerRequest(requestId, userId, fullName, volunteerInfo);
        return request;
    }
    
    public void TakeInReview(Guid adminId, Guid discussionId)
    {
        AdminId = adminId;
        DiscussionId = discussionId;
        Status = RequestStatus.OnReview;
    }
    
    public void SetRevisionRequiredStatus(
        RejectionComment rejectedComment)
    {
        Status = RequestStatus.RevisionRequired;
        RejectionComment = rejectedComment;
    }

    public void SetApprovedStatus()
    {
        Status = RequestStatus.Approved;
    }
    
    public void SetRejectStatus(RejectionComment rejectedComment)
    {
        RejectionComment = rejectedComment;
        Status = RequestStatus.Rejected;
    }
    
    public void Refresh()
    {
        Status = RequestStatus.Submitted;
    }
}