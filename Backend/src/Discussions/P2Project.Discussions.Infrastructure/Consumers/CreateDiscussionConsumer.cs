using MassTransit;
using Microsoft.Extensions.Logging;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Domain.ValueObjects;
using P2Project.VolunteerRequests.Agreements.Messages;

namespace P2Project.Discussions.Infrastructure.Consumers;

public class CreateDiscussionConsumer :
    IConsumer<VolunteerRequestReviewStartedEvent>
{
    private readonly IDiscussionsRepository _discussionRepository;
    private readonly ILogger<CreateDiscussionConsumer> _logger;

    public CreateDiscussionConsumer(
        IDiscussionsRepository discussionRepository,
        ILogger<CreateDiscussionConsumer> logger)
    {
        _discussionRepository = discussionRepository;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<VolunteerRequestReviewStartedEvent> context)
    {
        var discussionExist = await _discussionRepository.GetByParticipantsId(
            context.Message.ReviewingUserId,
            context.Message.ApplicantUserId,
            context.CancellationToken);
        if (discussionExist.IsSuccess)
            throw new Exception(discussionExist.Error.Message);
        
        var discussionUsers = DiscussionUsers.Create(
            context.Message.ReviewingUserId,
            context.Message.ApplicantUserId);

        var discussion = Discussion.Open(
            context.Message.RequesterId,
            discussionUsers);

        if (discussion.IsFailure)
            throw new Exception(discussion.Error.Message);

        await _discussionRepository.Add(discussion.Value, context.CancellationToken);
        
        _logger.LogInformation(
            "Discussion was open with id {Id}", discussion.Value.Id);
    }
}