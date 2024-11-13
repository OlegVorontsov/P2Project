using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.Volunteers.CreateVolunteer;

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
                return validationResult.ToValidarionErrorResponse();

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}