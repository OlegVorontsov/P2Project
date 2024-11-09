using CSharpFunctionalExtensions;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.ValueObjects;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        public async Task<Result<Guid, string>> Handle(CreateVolunteerRequest request,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var emailResult = Email.Create(request.Email);
            if(emailResult.IsFailure)
                return emailResult.Error;

            var descriptionResult = Description.Create(request.Description);
            if (descriptionResult.IsFailure)
                return descriptionResult.Error;

            var volunteerResult = Volunteer.Create(volunteerId,
                                             emailResult.Value,
                                             descriptionResult.Value);

            return (Guid)volunteerResult.Value.Id;
        }
    }
}
