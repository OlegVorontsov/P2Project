using FilesService.Core.ErrorManagment;
using FilesService.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Core.Extensions;

public static class ResponseExtension
{
    public static ActionResult MakeResponse(this Error error)
    {
        var statusCode = GetStatusCodeForErrorType(error.Type);

        var envelope = ResponseEnvelope.Error(error.ToErrorList());
        return new ObjectResult(envelope) { StatusCode = statusCode };
    }
    
    public static ActionResult MakeResponse(this ErrorList errors)
    {
        if (!errors.Any())
        {
            return new ObjectResult(ResponseEnvelope.Error(errors))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var distinctErrorTypes = errors
            .Select(e => e.Type)
            .Distinct()
            .ToList();

        var statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctErrorTypes.First());

        var envelope = ResponseEnvelope.Error(errors);

        return new ObjectResult(envelope) { StatusCode = statusCode };
    }
    
    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
}