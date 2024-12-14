using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers;

public record UpdatePhoneNumbersRequest(
    Guid VolunteerId,
    IEnumerable<PhoneNumberDto> PhoneNumbers)
{
    public UpdatePhoneNumbersCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, PhoneNumbers);
}
