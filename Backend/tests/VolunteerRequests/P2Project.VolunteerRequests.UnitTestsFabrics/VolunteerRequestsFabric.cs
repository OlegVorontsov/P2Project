using P2Project.SharedKernel;
using P2Project.SharedKernel.ValueObjects;
using P2Project.VolunteerRequests.Domain;
using P2Project.Volunteers.Domain;

namespace P2Project.VolunteerRequests.UnitTestsFabrics;

public static class VolunteerRequestsFabric
{
    static Random random = new Random();

    public static VolunteerRequest CreateTestRequest()
    {
        var fullName = FullName.Create("FirstName", "SecondName", "LastName").Value;
        
        var volunteerInfo = VolunteerInfo.Create(
            random.Next(Constants.MIN_AGE, Constants.MAX_AGE),
            random.Next(Constants.MIN_GRADE, Constants.MAX_GRADE)).Value;
        
        var userId = Guid.NewGuid();
        
        var request = VolunteerRequest.Create(
            Guid.NewGuid(), userId, fullName, volunteerInfo, Gender.Male);
        
        return request.Value;
    }
}