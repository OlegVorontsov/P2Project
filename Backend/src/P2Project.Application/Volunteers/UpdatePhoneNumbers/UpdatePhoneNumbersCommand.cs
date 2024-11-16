namespace P2Project.Application.Volunteers.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersCommand(
        Guid VolunteerId,
        UpdatePhoneNumbersDto PhoneNumbersDto);
}
