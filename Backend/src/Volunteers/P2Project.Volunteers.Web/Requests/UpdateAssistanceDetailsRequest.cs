using P2Project.Core.Dtos.Common;
using P2Project.Volunteers.Application.Commands.UpdateAssistanceDetails;

namespace P2Project.Volunteers.Web.Requests;

public record UpdateAssistanceDetailsRequest(
    IEnumerable<AssistanceDetailDto> AssistanceDetails)
{
    public UpdateAssistanceDetailsCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, AssistanceDetails);
}
