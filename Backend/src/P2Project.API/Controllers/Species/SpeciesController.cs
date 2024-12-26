using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Species.Requests;
using P2Project.API.Extensions;
using P2Project.Application.Species.Commands.AddBreeds;
using P2Project.Application.Species.Commands.Create;
using P2Project.Application.Species.Commands.DeleteSpeciesById;

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
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSpecies(
            [FromRoute] Guid id,
            [FromServices] DeleteSpeciesByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                new DeleteSpeciesByIdCommand(id), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
