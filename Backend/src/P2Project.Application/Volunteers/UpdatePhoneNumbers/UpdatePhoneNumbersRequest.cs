
namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersRequest(
        Guid VolunteerId,
        UpdatePhoneNumbersDto PhoneNumbersDto);
}
