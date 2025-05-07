using CSharpFunctionalExtensions;
using P2Project.Core.Events;
using P2Project.SharedKernel.BaseClasses;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using P2Project.VolunteerRequests.Domain.Enums;
using P2Project.VolunteerRequests.Domain.ValueObjects;
using P2Project.Volunteers.Domain;

namespace P2Project.VolunteerRequests.Domain;

public class VolunteerRequest : DomainEntity<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id) : base(id) {}
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public FullName FullName { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    public Gender Gender { get; private set; }

    private VolunteerRequest(
        VolunteerRequestId requestId,
        Guid userId,
        FullName fullName,
        VolunteerInfo volunteerInfo,
        Gender gender) : base(requestId)
    {
        Id = requestId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Submitted;
        FullName = fullName;
        VolunteerInfo = volunteerInfo;
        Gender = gender;
    }
    
    public static Result<VolunteerRequest, Error> Create(
        VolunteerRequestId requestId,
        Guid userId,
        FullName fullName,
        VolunteerInfo volunteerInfo,
        Gender gender) =>
        new VolunteerRequest(requestId, userId, fullName, volunteerInfo, gender);
    
    public void TakeInReview(Guid adminId)
    {
        AdminId = adminId;
        Status = RequestStatus.OnReview;
        AddDomainEvent(new ReviewStartedEvent(Id, adminId, UserId, FullName.FirstName));
    }
    
    public void SetRevisionRequiredStatus(
        Guid adminId,
        RejectionComment rejectedComment)
    {
        Status = RequestStatus.RevisionRequired;
        RejectionComment = rejectedComment;
        AddDomainEvent(new CreateMessageEvent(Id, adminId, UserId, FullName.FirstName, rejectedComment.Value));
    }

    public void SetApprovedStatus(Guid adminId, string comment)
    {
        Status = RequestStatus.Approved;
        RejectionComment = null;
        AddDomainEvent(new ApprovedEvent(
            UserId, FullName.FirstName, VolunteerInfo.Age, VolunteerInfo.Grade, Gender.ToString()));
        AddDomainEvent(new CreateMessageEvent(Id, adminId, UserId, FullName.FirstName, comment));
    }
    
    public void SetRejectStatus(
        Guid adminId,
        RejectionComment rejectedComment)
    {
        Status = RequestStatus.Rejected;
        RejectionComment = rejectedComment;
        AddDomainEvent(new CreateMessageEvent(Id, adminId, UserId, FullName.FirstName, rejectedComment.Value));
    }
    
    public void Refresh(
        Guid adminId, string message)
    {
        Status = RequestStatus.Submitted;
        AddDomainEvent(new CreateMessageEvent(Id, adminId, UserId, FullName.FirstName, message));
    }
}