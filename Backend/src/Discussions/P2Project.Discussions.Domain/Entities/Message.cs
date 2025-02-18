using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Domain.Entities;

public class Message
{
    private Message() { }
    
    public Guid Id { get; private set; }
    public Content Content { get; private set; }
    public Guid SenderId { get; private set; } 
    public Guid DiscussionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;
    
    private Message(Guid discussionId, Guid senderId, Content content)
    {
        CreatedAt = DateTime.UtcNow;
        Content = content;
        SenderId = senderId;
        DiscussionId = discussionId;
    }

    public static Message Create(Guid discussionId, Guid senderId, Content content) 
        => new (discussionId, senderId, content);

    public void EditMessage(Content newContent)
    {
        Content = newContent;
        IsEdited = true;
    }
}