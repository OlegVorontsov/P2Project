using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Response;
using P2Project.Domain.Shared;

namespace P2Project.API.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = GetStatusCodeForError(error);

            var responseError = new ResponseError(error.Code, error.Message, null);

            var envelope = Envelope.Error([responseError]);
            return new ObjectResult(envelope)
            {
                StatusCode = statusCode
            };
        }
        public static int GetStatusCodeForError(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
            return statusCode;
        }
    }
}
