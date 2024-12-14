using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersCommand(
        Guid VolunteerId,
        IEnumerable<PhoneNumberDto> PhoneNumbers);
}
