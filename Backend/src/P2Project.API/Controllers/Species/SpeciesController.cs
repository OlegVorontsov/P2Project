using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Species.Requests;
using P2Project.API.Extensions;
using P2Project.Application.Species.Commands.AddBreeds;
using P2Project.Application.Species.Commands.Create;
using P2Project.Application.Species.Commands.DeleteBreedById;
using P2Project.Application.Species.Commands.DeleteSpeciesById;
using P2Project.Application.Species.Queries.GetAllSpeciesFilteredPaginated;

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
        
        [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
        public async Task<IActionResult> DeleteBreed(
            [FromRoute] Guid speciesId,
            [FromRoute] Guid breedId,
            [FromServices] DeleteBreedByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                new DeleteBreedByIdCommand(speciesId, breedId),
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllSpecies(
            [FromQuery] GetAllSpeciesFilteredPaginatedRequest request,
            [FromServices] GetAllSpeciesFilteredPaginatedQueryHandler handler,
            CancellationToken cancellationToken = default)
        {
            var species = await handler.Handle(
                request.ToQuery(), cancellationToken);

            return Ok(species.Value);
        }
    }
}
