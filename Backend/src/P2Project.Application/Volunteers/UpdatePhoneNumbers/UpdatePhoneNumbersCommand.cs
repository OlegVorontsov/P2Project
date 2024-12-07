using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersCommand(
        Guid VolunteerId,
        IEnumerable<PhoneNumberDto> PhoneNumbers);
}
