using P2Project.Discussions.Domain;

namespace P2Project.Discussions.Application.Interfaces;

public interface IDiscussionsRepository
{
    public Task<Discussion> Add(Discussion discussion, CancellationToken cancellationToken);
}