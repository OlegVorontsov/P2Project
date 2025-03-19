using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Agreements;

public interface IDiscussionsAgreement
{
    public Task<Result<Guid, ErrorList>> CreateMessage(
        Guid senderId, Guid participantId,
        string message,
        CancellationToken cancellationToken = default);
}