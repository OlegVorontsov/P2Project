using CSharpFunctionalExtensions;
using P2Project.Discussions.Agreements;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.Create;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Web;

public class DiscussionsAgreement : IDiscussionsAgreement
{
    private readonly CreateDiscussionHandler _createDiscussionHandler;

    public DiscussionsAgreement(CreateDiscussionHandler createDiscussionHandler)
    {
        _createDiscussionHandler = createDiscussionHandler;
    }
    public async Task<Result<Guid, ErrorList>> CreateDiscussion(
        Guid reviewingUserId,
        Guid applicantUserId,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateDiscussionCommand(reviewingUserId, applicantUserId);
        
        var discussion = await _createDiscussionHandler.Handle(createCommand, cancellationToken);
        if (discussion.IsFailure)
            return discussion.Error;
        
        return discussion.Value.DiscussionId;
    }
}