using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Species.Requests;
using P2Project.API.Extensions;
using P2Project.Application.Species.AddBreeds;
using P2Project.Application.Species.Create;

namespace P2Project.API.Controllers.Species
{
    public class SpeciesController : ApplicationController
    {
        [HttpPost()]
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateSpeciesRequest request,
            [FromServices] CreateHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/breeds")]
        public async Task<ActionResult<Guid>> AddBreeds(
            [FromRoute] Guid id,
            [FromBody] AddBreedsRequest request,
            [FromServices] AddBreedsHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
