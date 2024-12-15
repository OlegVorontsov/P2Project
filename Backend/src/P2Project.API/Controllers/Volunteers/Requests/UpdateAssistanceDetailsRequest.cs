using P2Project.Application.Shared.Dtos;
using P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails;

namespace P2Project.API.Controllers.Volunteers.Requests;

public record UpdateAssistanceDetailsRequest(
    Guid VolunteerId,
    IEnumerable<AssistanceDetailDto> AssistanceDetails)
{
    public UpdateAssistanceDetailsCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, AssistanceDetails);
}
