using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.Species.Create;

namespace P2Project.API.Controllers
{
    public class SpeciesController : ApplicationController
    {
        [HttpPost()]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateHandler handler,
            [FromBody] CreateSpeciesRequest request,
            [FromServices] IValidator<CreateSpeciesRequest> validator,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(new CreateCommand(
                request.Name,
                request.Breeds), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
            //return Ok(Guid.NewGuid());
        }
    }
}
