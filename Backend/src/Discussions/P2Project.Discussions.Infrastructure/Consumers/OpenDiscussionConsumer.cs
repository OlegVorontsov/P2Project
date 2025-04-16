using MassTransit;
using Microsoft.Extensions.Logging;
using P2Project.Core.Outbox.Messages.VolunteerRequests;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Domain.ValueObjects;

namespace P2Project.Discussions.Infrastructure.Consumers;

public class OpenDiscussionConsumer(
    IDiscussionsRepository discussionRepository,
    ILogger<OpenDiscussionConsumer> logger) : IConsumer<OpenDiscussionEvent>
{
    public async Task Consume(ConsumeContext<OpenDiscussionEvent> context)
    {
        var discussionExist = await discussionRepository.GetByParticipantsId(
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

        await discussionRepository.Add(discussion.Value, context.CancellationToken);
        
        logger.LogInformation(
            "Discussion was open with id {Id}", discussion.Value.Id);
    }
}