using CSharpFunctionalExtensions;
using P2Project.Discussions.Domain.Entities;
using P2Project.Discussions.Domain.ValueObjects;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Domain;

public class Discussion
{
    private Discussion() { }
    private readonly List<Message> _messages = [];

    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public IReadOnlyList<Message> Messages => _messages;
    public DiscussionUsers DiscussionUsers { get; } = default!;
    public DiscussionStatus Status { get; private set; }
    
    private Discussion(
        Guid requestId,
        DiscussionUsers discussionUsers)
    {
        RequestId = requestId;
        DiscussionUsers = discussionUsers;
        Status = DiscussionStatus.Open;
    }
    
    public void Close() => Status = DiscussionStatus.Closed;
    public void Reopen() => Status = DiscussionStatus.Open;
    
    public static Result<Discussion, Error> Open(
        Guid requestId,
        DiscussionUsers discussionUsers)
    {
        var discussion = new Discussion(requestId, discussionUsers)
        {
            Status = DiscussionStatus.Open
        };
        return discussion;
    }
    
    public UnitResult<Error> AddMessage(Message message)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.Discussions.Failure(Constants.Discussions.IS_CLOSED);

        if (IsFromUserInDiscussion(message) == false)
            return Errors.Discussions.Failure(Constants.Discussions.USER_NOT_IN);

        _messages.Add(message);
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> EditMessage(
        Guid messageId, Guid userId, Content content)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.Discussions.Failure(Constants.Discussions.IS_CLOSED);

        var message = _messages.FirstOrDefault(m => m.Id == messageId);
        if (message is null)
            return Errors.General.NotFound();
        
        if (message.SenderId != userId)
            return Errors.Discussions.Failure(Constants.Discussions.NOT_USERS_MESSAGE);
        
        message.EditMessage(content);
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> RemoveMessage(Guid messageId, Guid userId)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.Discussions.Failure(Constants.Discussions.IS_CLOSED);

        var message = _messages.FirstOrDefault(m => m.Id == messageId);
        if (message is null)
            return Errors.General.NotFound();
        
        if (message.SenderId != userId)
            return Errors.Discussions.Failure(Constants.Discussions.NOT_USERS_MESSAGE);
        
        _messages.Remove(message);
        return Result.Success<Error>();
    }

    private bool IsFromUserInDiscussion(Message message)
    {
        return DiscussionUsers.ApplicantUserId == message.SenderId ||
               DiscussionUsers.ReviewingUserId == message.SenderId;
    }
}