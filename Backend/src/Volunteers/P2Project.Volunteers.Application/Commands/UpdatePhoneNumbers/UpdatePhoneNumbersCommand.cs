using P2Project.Core.Dtos.Common;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersCommand(
        Guid VolunteerId,
        IEnumerable<PhoneNumberDto> PhoneNumbers) : ICommand;
}
