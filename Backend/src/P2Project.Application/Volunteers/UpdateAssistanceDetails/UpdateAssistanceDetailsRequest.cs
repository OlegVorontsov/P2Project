using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdateAssistanceDetails;

public record UpdateAssistanceDetailsRequest(
    Guid VolunteerId,
    IEnumerable<AssistanceDetailDto> AssistanceDetails)
{
    public UpdateAssistanceDetailsCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, AssistanceDetails);
}
