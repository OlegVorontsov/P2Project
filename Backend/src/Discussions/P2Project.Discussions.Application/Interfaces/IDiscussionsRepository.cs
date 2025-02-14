using CSharpFunctionalExtensions;
using P2Project.Discussions.Domain;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.Interfaces;

public interface IDiscussionsRepository
{
    public Task<Discussion> Add(Discussion discussion, CancellationToken cancellationToken);

    public Task<Result<Discussion, Error>> GetByParticipantId(
        Guid participantId,
        CancellationToken cancellationToken);
}