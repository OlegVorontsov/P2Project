using P2Project.Core.Errors;

namespace P2Project.Core.Models
{
    public record ResponseError(
                    string? ErrorCode,
                    string? ErrorMessage,
                    string? InvalidField);
    public record Envelope
    {
        public object? Result { get; }
        public ErrorList? Errors { get; }
        public List<ResponseError> ResponseErrors { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(
                object? result,
                ErrorList? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }
        private Envelope(
            object? result,
            IEnumerable<ResponseError> errors)
        {
            Result = result;
            ResponseErrors = errors.ToList();
            TimeGenerated = DateTime.Now;
        }
        public static Envelope Ok(object? result = null) =>
            new (result, []);
        public static Envelope Error(ErrorList errors) =>
            new (null, errors);
        public static Envelope ResponseError(IEnumerable<ResponseError> errors) =>
            new(null, errors);
    }
}
