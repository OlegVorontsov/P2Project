using CSharpFunctionalExtensions;
using System.Linq;

namespace P2Project.Domain.ValueObjects
{
    public class AssistanceStatus : ValueObject
    {
        private readonly List<AssistanceStatus> _statusList = [NeedsHelp,
                                                               NeedsFood,
                                                               OnMedication,
                                                               LooksForHome,
                                                               FoundHome];
        private AssistanceStatus(string status)
        {
            Status = status;
            _statusList.Add(this);
        }
        public string Status { get; } = default!;
        public IReadOnlyList<AssistanceStatus> StatusList => _statusList;
        public static AssistanceStatus NeedsHelp { get; } = default!;
        public static AssistanceStatus NeedsFood { get; } = default!;
        public static AssistanceStatus OnMedication { get; } = default!;
        public static AssistanceStatus LooksForHome { get; } = default!;
        public static AssistanceStatus FoundHome { get; } = default!;
        public static Result<AssistanceStatus> Create(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Result.Failure<AssistanceStatus>("Status can't be empty");

            var newStatus = new AssistanceStatus(status);

            return Result.Success(newStatus);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
