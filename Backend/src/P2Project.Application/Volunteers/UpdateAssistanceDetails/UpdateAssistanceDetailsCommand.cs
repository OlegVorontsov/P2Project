using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Volunteers.UpdateAssistanceDetails
{
    public record UpdateAssistanceDetailsCommand(
        Guid VolunteerId,
        IEnumerable<AssistanceDetailDto> AssistanceDetails);
}
