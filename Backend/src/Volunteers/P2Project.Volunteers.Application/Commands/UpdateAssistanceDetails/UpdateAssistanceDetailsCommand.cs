using P2Project.Core.Dtos.Common;
using P2Project.Core.Interfaces.Commands;

namespace P2Project.Volunteers.Application.Commands.UpdateAssistanceDetails
{
    public record UpdateAssistanceDetailsCommand(
        Guid VolunteerId,
        IEnumerable<AssistanceDetailDto> AssistanceDetails) : ICommand;
}
