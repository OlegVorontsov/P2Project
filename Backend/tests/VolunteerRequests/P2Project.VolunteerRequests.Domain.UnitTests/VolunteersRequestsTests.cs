using P2Project.SharedKernel.ValueObjects;
using P2Project.VolunteerRequests.Domain.Enums;
using P2Project.VolunteerRequests.Domain.ValueObjects;
using P2Project.VolunteerRequests.UnitTestsFabrics;
using P2Project.Volunteers.Domain;

namespace P2Project.VolunteerRequests.Domain.UnitTests;

public class VolunteersRequestsTests
{
    [Fact]
    public void CreateRequest_Should_Be_Not_Null()
    {
        // Arrange
        var fullName = FullName.Create("FirstName", "SecondName", "LastName").Value;
        var volunteerInfo = VolunteerInfo.Create(55, 7).Value;
        var userId = Guid.NewGuid();
        
        // Act
        var request = VolunteerRequest.Create(
            Guid.NewGuid(), userId, fullName, volunteerInfo, Gender.Male);
        
        //Assert
        Assert.NotNull(request);
        Assert.Equal(userId, request.Value.UserId);
        Assert.Equal(RequestStatus.Submitted, request.Value.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_Rejected()
    {
        // Arrange
        var request = VolunteerRequestsFabric.CreateTestRequest();
        
        // Act
        request.SetRejectStatus(
            Guid.NewGuid(),
            RejectionComment.Create("Rejection Comment").Value);
        
        //Assert
        Assert.Equal(RequestStatus.Rejected, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_RevisionRequired()
    {
        // Arrange
        var request = VolunteerRequestsFabric.CreateTestRequest();
        
        // Act
        request.SetRevisionRequiredStatus(
            Guid.NewGuid(),
            RejectionComment.Create("Rejection Comment").Value);
        
        //Assert
        Assert.Equal(RequestStatus.RevisionRequired, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_Approved()
    {
        // Arrange
        var request = VolunteerRequestsFabric.CreateTestRequest();
        
        // Act
        request.SetApprovedStatus(Guid.NewGuid(), "message");
        
        //Assert
        Assert.Equal(RequestStatus.Approved, request.Status);
    }
    
    [Fact]
    public void ChangeStatus_To_OnReview()
    {
        // Arrange
        var request = VolunteerRequestsFabric.CreateTestRequest();
        
        // Act
        request.TakeInReview(Guid.NewGuid());
        
        //Assert
        Assert.Equal(RequestStatus.OnReview, request.Status);
    }
}