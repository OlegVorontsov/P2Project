using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.API.Response;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Domain.Shared;

namespace P2Project.API.Controllers
{
    public class VolunteerController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromServices] IValidator<CreateCommand> validator,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateCommand(
                    request.FullName,
                    request.Age,
                    request.Gender,
                    request.Email,
                    request?.Description,
                    request.PhoneNumbers,
                    request?.SocialNetworks,
                    request?.AssistanceDetails);

            var validationResult = await validator.ValidateAsync(
                                        command,
                                        cancellationToken);

            if (validationResult.IsValid == false)
            {
                var validationErrors = validationResult.Errors;

                List<ResponseError> errors = [];

                foreach (var validationError in validationErrors)
                {
                    var error = Error.Validation(
                                      validationError.ErrorCode,
                                      validationError.ErrorMessage);

                    var responseError = new ResponseError(
                                            error.Code,
                                            error.Message,
                                            validationError.PropertyName);
                    errors.Add(responseError);
                };
                var envelope = Envelope.Error(errors);

                return BadRequest(envelope);
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}