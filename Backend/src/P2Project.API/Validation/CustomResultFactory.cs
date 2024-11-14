using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using P2Project.API.Response;
using P2Project.Domain.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace P2Project.API.Validation
{
    public class CustomResultFactory :
        IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(
                    ActionExecutingContext context,
                    ValidationProblemDetails? validationProblemDetails)
        {
            if (validationProblemDetails is null)
                throw new InvalidOperationException(
                    "ValidationProblemDetails is null");

            List<ResponseError> responseErrors = [];

            foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
            {
                foreach (var validationError in validationErrors)
                {
                    var error = Error.Deserialize(validationError);

                    var responseError = new ResponseError(
                                            error.Code,
                                            error.Message,
                                            invalidField);

                    responseErrors.Add(responseError);
                }
            }
            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
