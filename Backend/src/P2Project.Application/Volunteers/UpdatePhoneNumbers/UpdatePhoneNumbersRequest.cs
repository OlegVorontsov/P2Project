using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersRequest(
        Guid VolunteerId,
        UpdatePhoneNumbersDto PhoneNumbersDto);

    public record UpdatePhoneNumbersDto(
        IEnumerable<PhoneNumberDto> PhoneNumbers);
}
