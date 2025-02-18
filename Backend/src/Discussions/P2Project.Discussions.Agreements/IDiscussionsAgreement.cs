using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Agreements;

public interface IDiscussionsAgreement
{
    public Task<Result<Guid, ErrorList>> CreateDiscussion(Guid reviewingUserId,
        Guid applicantUserId,
        CancellationToken cancellationToken = default);
    
    public Task<Result<Guid, ErrorList>> CreateMessage(
        Guid senderId, Guid reviewingUserId,
        string message,
        CancellationToken cancellationToken = default);
}