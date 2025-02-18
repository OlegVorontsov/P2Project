using FluentAssertions;
using P2Project.Discussions.Domain.Entities;
using P2Project.Discussions.Domain.ValueObjects;
using P2Project.Discussions.UnitTestsFabrics;
using P2Project.SharedKernel;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Domain.UnitTests;

public class DiscussionsTests
{
    [Fact]
    public void OpenDiscussion_Should_Be_Not_Null()
    {
        // Arrange
        var discussionUsers = DiscussionUsers.Create(Guid.NewGuid(), Guid.NewGuid());
        
        // Act
        var discussion = Discussion.Open(discussionUsers).Value;
        
        //Assert
        Assert.NotNull(discussion);
    }
    
    [Fact]
    public void CloseDiscussion_Status_Should_Be_Closed()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        
        // Act
        discussion.Close();
        
        //Assert
        Assert.Equal(DiscussionStatus.Closed, discussion.Status);
    }
    
    [Fact]
    public void ReopenDiscussion_Status_Should_Be_Open()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        
        // Act
        discussion.Close();
        discussion.Reopen();
        
        //Assert
        Assert.Equal(DiscussionStatus.Open, discussion.Status);
    }
    
    [Fact]
    public void Add_Message_To_Discussion_Should_Be_Not_Null()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = Content.Create("Test content").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        
        // Act
        var result = discussion.AddMessage(message);
        var messageInDiscussion = discussion.Messages.First().Content;
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        Assert.Single(discussion.Messages);
        messageText.Value.Should().Be(messageInDiscussion);
    }
    
    [Fact]
    public void Add_Message_To_Discussion_When_User_Is_Not_In_Should_Be_Failure()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        var messageText = Content.Create("Test content").Value;
        var message = Message.Create(discussion.DiscussionId, Guid.NewGuid(), messageText);
        
        // Act
        var result = discussion.AddMessage(message);
        
        //Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void Edit_Message_In_Discussion_When_User_Is_Not_In_Should_Be_Failure()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = Content.Create("Test content").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        discussion.AddMessage(message);
        var newMessageText = Content.Create("New Message").Value;
        
        // Act
        var result = discussion.EditMessage(message.Id, Guid.NewGuid(), newMessageText);
        
        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(Constants.Discussions.NOT_USERS_MESSAGE);
    }
    
    [Fact]
    public void Remove_Message_From_Discussion_When_User_Is_Not_In_Discussion_Should_Be_Failure()
    {
        // Arrange
        var discussion = DiscussionsFabric.OpenDiscussion();
        var userId = discussion.DiscussionUsers.ApplicantUserId;
        var messageText = Content.Create("Test content").Value;
        var message = Message.Create(discussion.DiscussionId, userId, messageText);
        discussion.AddMessage(message);
        
        // Act
        var result = discussion.RemoveMessage(message.Id, Guid.NewGuid());
        
        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be(Constants.Discussions.NOT_USERS_MESSAGE);
    }
}