using CSharpFunctionalExtensions;
using P2Project.Core.Errors;

namespace P2Project.Volunteers.Domain.ValueObjects.Pets
{
    public class AssistanceStatus
    {
        public const string DB_COLUMN_ASSISTANCE_STATUS = "assistance_status";

        private AssistanceStatus() { }
        private AssistanceStatus(string status)
        {
            Status = status;
        }
        public string Status { get; } = default!;

        public static Result<AssistanceStatus, Error> Create(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Errors.General.ValueIsInvalid(nameof(Status));

            var newStatus = new AssistanceStatus(status.ToLower());

            return newStatus;
        }
    }
}
