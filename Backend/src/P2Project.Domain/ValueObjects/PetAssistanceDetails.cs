
namespace P2Project.Domain.ValueObjects
{
    public record PetAssistanceDetails
    {
        private PetAssistanceDetails() { }
        public PetAssistanceDetails(IReadOnlyList<AssistanceDetail>? assistanceDetails)
        {
            AssistanceDetails = assistanceDetails;
        }
        public IReadOnlyList<AssistanceDetail>? AssistanceDetails { get; } = default!;
    }
}
