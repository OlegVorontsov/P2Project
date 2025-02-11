using P2Project.Discussions.Domain;
using P2Project.Discussions.Domain.ValueObjects;

namespace P2Project.Discussions.UnitTestsFabrics;

public static class DiscussionsFabric
{
    public static Discussion OpenDiscussion()
    {
        var discussionUsers = DiscussionUsers.Create(Guid.NewGuid(), Guid.NewGuid());
        var discussion = Discussion.Open(discussionUsers).Value;
        
        return discussion;
    }
}