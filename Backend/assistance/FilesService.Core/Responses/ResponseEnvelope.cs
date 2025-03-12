using FilesService.Core.ErrorManagment;

namespace FilesService.Core.Responses;
public record ErrorResponse(
    string? ErrorCode,
    string? ErrorMessage,
    string? InvalidField);
public record ResponseEnvelope
{
    public object? Result { get; }
    public ErrorList? Errors { get; }
    public List<ErrorResponse> ResponseErrors { get; }
    public DateTime TimeGenerated { get; }

    private ResponseEnvelope(
        object? result,
        ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }
    private ResponseEnvelope(
        object? result,
        IEnumerable<ErrorResponse> errors)
    {
        Result = result;
        ResponseErrors = errors.ToList();
        TimeGenerated = DateTime.Now;
    }
    public static ResponseEnvelope Error(ErrorList errors) =>
        new (null, errors);
}