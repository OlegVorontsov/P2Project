using P2Project.Application.Interfaces;
using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Common;

namespace P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails
{
    public record UpdateAssistanceDetailsCommand(
        Guid VolunteerId,
        IEnumerable<AssistanceDetailDto> AssistanceDetails) : ICommand;
}
