using P2Project.Core.Dtos.Discussions;

namespace P2Project.Discussions.Application;

public interface IDiscussionsReadDbContext
{
    IQueryable<DiscussionDto> Discussions { get; }
    IQueryable<MessageDto> Messages { get; }
}