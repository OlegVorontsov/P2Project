using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Infrastructure.DbContexts;

namespace P2Project.Discussions.Infrastructure;

public class DiscussionsRepository : IDiscussionsRepository
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public DiscussionsRepository(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Discussion> Add(
        Discussion discussion, CancellationToken cancellationToken)
    {
        _dbContext.Discussions.Add(discussion);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return discussion;
    }
}