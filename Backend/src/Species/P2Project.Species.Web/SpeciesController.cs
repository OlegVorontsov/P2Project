using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.Species.Application.Commands.AddBreeds;
using P2Project.Species.Application.Commands.Create;
using P2Project.Species.Application.Commands.DeleteBreedById;
using P2Project.Species.Application.Commands.DeleteSpeciesById;
using P2Project.Species.Application.Queries.GetAllBreedsPaginatedBySpeciesId;
using P2Project.Species.Application.Queries.GetAllSpeciesFilteredPaginated;
using P2Project.Species.Web.Requests;

namespace P2Project.Species.Web
{
    [Authorize]
    public class SpeciesController : ApplicationController
    {
        [Permission(PermissionsConfig.Species.Create)]
        [HttpPost]
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

        [Permission(PermissionsConfig.Breeds.Create)]
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
        
        [Permission(PermissionsConfig.Species.Delete)]
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
        
        [Permission(PermissionsConfig.Breeds.Delete)]
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
        
        [Permission(PermissionsConfig.Species.Read)]
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
        
        [Permission(PermissionsConfig.Species.Read)]
        [HttpGet("{id:guid}/breeds")]
        public async Task<IActionResult> GetBreedsBySpeciesId(
            [FromRoute] Guid id,
            [FromQuery] GetAllBreedsPaginatedBySpeciesIdRequest request,
            [FromServices] GetAllBreedsPaginatedBySpeciesIdHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToQuery(id), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
