namespace P2Project.Application.Volunteers.UpdateAssistanceDetails
{
    public record UpdateAssistanceDetailsCommand(
        Guid VolunteerId,
        UpdateAssistanceDetailsDto AssistanceDetailsDto);

}
