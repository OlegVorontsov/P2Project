using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Common;

namespace P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers
{
    public record UpdatePhoneNumbersCommand(
        Guid VolunteerId,
        IEnumerable<PhoneNumberDto> PhoneNumbers) : ICommand;
}
