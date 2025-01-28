using P2Project.Core.Dtos.Common;
using P2Project.Core.Dtos.Volunteers;
using P2Project.Volunteers.Application.Commands.Create;

namespace P2Project.Volunteers.Web.Requests;

public record CreateRequest(
            VolunteerInfoDto VolunteerInfo,
            string Gender,
            string? Description,
            IEnumerable<PhoneNumberDto> PhoneNumbers)
{
    public CreateCommand ToCommand() =>
        new(VolunteerInfo,
            Gender,
            Description,
            PhoneNumbers);
}
