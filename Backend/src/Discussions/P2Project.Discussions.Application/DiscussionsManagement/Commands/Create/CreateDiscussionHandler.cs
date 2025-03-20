using MediatR;
using Microsoft.Extensions.Logging;
using P2Project.Core.Events;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Domain.ValueObjects;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Create;

public class CreateDiscussionHandler :
    INotificationHandler<CreateDiscussionEvent>
{
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly ILogger<CreateDiscussionHandler> _logger;

    public CreateDiscussionHandler(
        IDiscussionsRepository discussionsRepository,
        ILogger<CreateDiscussionHandler> logger)
    {
        _discussionsRepository = discussionsRepository;
        _logger = logger;
    }

    public async Task Handle(
        CreateDiscussionEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var discussionExist = await _discussionsRepository.GetByParticipantsId(
            domainEvent.ReviewingUserId, domainEvent.ApplicantUserId, cancellationToken);
        if (discussionExist.IsSuccess)
            throw new Exception(discussionExist.Error.Message);
        
        var discussionUsers = DiscussionUsers.Create(
            domainEvent.ReviewingUserId, domainEvent.ApplicantUserId);

        var discussion = Discussion.Open(domainEvent.RequestId, discussionUsers);

        if (discussion.IsFailure)
            throw new Exception(discussion.Error.Message);

        await _discussionsRepository.Add(discussion.Value, cancellationToken);
        
        _logger.LogInformation(
            "Discussion was open with id {Id}", discussion.Value.Id);
    }
}