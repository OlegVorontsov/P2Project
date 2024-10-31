using CSharpFunctionalExtensions;

namespace P2Project.Domain.ValueObjects
{
    public class AssistanceStatus : ValueObject
    {
        public string Status { get; }
        private AssistanceStatus(string status)
        {
            Status = status;
        }
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
