using Microsoft.AspNetCore.Mvc;
using P2Project.API.Response;
using P2Project.Domain.Shared;

namespace P2Project.API.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = GetStatusCodeForErrorType(error.Type);

            var envelope = Envelope.Error(error.ToErrorList());
            return new ObjectResult(envelope) { StatusCode = statusCode };
        }
        public static ActionResult ToResponse(this ErrorList errors)
        {
            if (!errors.Any())
            {
                return new ObjectResult(Envelope.Error(errors))
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

            var envelope = Envelope.Error(errors);

            return new ObjectResult(envelope) { StatusCode = statusCode };
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
            var envelope = Envelope.ResponseError(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
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
}
