using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Common;
using P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record UpdatePhoneNumbersRequest(
    Guid VolunteerId,
    IEnumerable<PhoneNumberDto> PhoneNumbers)
{
    public UpdatePhoneNumbersCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, PhoneNumbers);
}
