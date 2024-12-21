using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Species.Requests;
using P2Project.API.Extensions;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Species.AddBreeds;
using P2Project.Application.Species.Create;

namespace P2Project.API.Controllers.Species
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
        }

        [HttpPatch("{id:guid}/breeds")]
        public async Task<ActionResult<Guid>> AddBreeds(
            [FromRoute] Guid id,
            [FromBody] AddBreedsDto dto,
            [FromServices] AddBreedsHandler handler,
            [FromServices] IValidator<AddBreedsRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new AddBreedsRequest(id, dto);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(
                new AddBreedsCommand(
                    request.SpeciesId,
                    request.AddBreedsDto), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
