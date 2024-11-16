using P2Project.Application.Dtos;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersDto(
        IEnumerable<PhoneNumberDto> PhoneNumbers);
}
