using FilesService.Core.ErrorManagment;

namespace FilesService.Core.Models
{
    public record ResponseError(
                    string? ErrorCode,
                    string? ErrorMessage,
                    string? InvalidField);
    public record FilesServiceEnvelope
    {
        public object? Result { get; }
        public ErrorList? Errors { get; }
        public List<ResponseError> ResponseErrors { get; }
        public DateTime TimeGenerated { get; }

        private FilesServiceEnvelope(
                object? result,
                ErrorList? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }
        private FilesServiceEnvelope(
            object? result,
            IEnumerable<ResponseError> errors)
        {
            Result = result;
            ResponseErrors = errors.ToList();
            TimeGenerated = DateTime.Now;
        }
        public static FilesServiceEnvelope Ok(object? result = null) =>
            new (result, []);
        public static FilesServiceEnvelope Error(ErrorList errors) =>
            new (null, errors);
        public static FilesServiceEnvelope ResponseError(IEnumerable<ResponseError> errors) =>
            new(null, errors);
    }
}
