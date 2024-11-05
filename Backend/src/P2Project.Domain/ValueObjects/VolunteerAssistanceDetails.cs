
namespace P2Project.Domain.ValueObjects
{
    public record VolunteerAssistanceDetails
    {
        private VolunteerAssistanceDetails() { }
        public VolunteerAssistanceDetails(IReadOnlyList<AssistanceDetail>? assistanceDetails)
        {
            AssistanceDetails = assistanceDetails;
        }
        public IReadOnlyList<AssistanceDetail>? AssistanceDetails { get; } = default!;
    }
}
