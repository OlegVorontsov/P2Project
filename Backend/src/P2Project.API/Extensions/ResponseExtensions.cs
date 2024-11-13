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
        public static ActionResult ToValidationErrorResponse(
            this FluentValidation.Results.ValidationResult result)
        {
            if (result.IsValid)
                throw new InvalidOperationException("Result can't be succeed");

            var validationErrors = result.Errors;

            List<ResponseError> responseErrors = [];

            foreach (var validationError in validationErrors)
            {
                var errorMessage = validationError.ErrorMessage;

                var error = Error.Deserialize(errorMessage);

                var responseError = new ResponseError(
                                        error.Code,
                                        error.Message,
                                        validationError.PropertyName);

                responseErrors.Add(responseError);
            };
            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status400BadRequest
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
