using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.VolunteerRequests.Domain.ValueObjects
{
    public class RejectionComment
    {
        private RejectionComment() { }
        private RejectionComment(string value)
        {
            Value = value;
        }
        public string Value { get; } = default!;

        public static Result<RejectionComment, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid(nameof(RejectionComment));

            var newComment = new RejectionComment(value);

            return newComment;
        }
    }
}
