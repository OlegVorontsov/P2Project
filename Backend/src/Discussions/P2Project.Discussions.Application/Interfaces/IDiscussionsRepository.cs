using CSharpFunctionalExtensions;
using P2Project.Discussions.Domain;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.Interfaces;

public interface IDiscussionsRepository
{
    public Task<Discussion> Add(Discussion discussion, CancellationToken cancellationToken);

    public Task<Result<Discussion, Error>> GetByParticipantsId(
        Guid ApplicantUserId, Guid ReviewingUserId,
        CancellationToken cancellationToken);

    public Task<Result<Discussion, Error>> GetById(
        Guid discussionId,
        CancellationToken cancellationToken);
}