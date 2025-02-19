using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Infrastructure.DbContexts;
using P2Project.SharedKernel.Errors;

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
    
    public async Task<Result<Discussion, Error>> GetByParticipantsId(
        Guid applicantUserId, Guid reviewingUserId,
        CancellationToken cancellationToken)
    {
        var discussion = await _dbContext.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.DiscussionUsers.ReviewingUserId == reviewingUserId &&
                                      d.DiscussionUsers.ApplicantUserId == applicantUserId, cancellationToken);

        if (discussion == null)
            return Errors.General.NotFound();
        
        return discussion;
    }
    
    public async Task<Result<Discussion, Error>> GetById(
        Guid discussionId,
        CancellationToken cancellationToken)
    {
        var discussion = await _dbContext.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.DiscussionId == discussionId, cancellationToken);

        if (discussion == null)
            return Errors.General.NotFound(discussionId);
        
        return discussion;
    }
}