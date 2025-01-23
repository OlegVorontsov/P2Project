using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Volunteers.Application.Queries.Pets.GetAllPets;
using P2Project.Volunteers.Application.Queries.Pets.GetPetById;
using P2Project.Volunteers.Web.Requests;

namespace P2Project.Volunteers.Web;

public class PetController : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetPetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var response = await handler.Handle(
            new GetPetByIdQuery(id), cancellationToken);

        return Ok(response.Value);
    }
    
    [HttpGet("dapper")]
    public async Task<IActionResult> GetByDapper(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsHandlerDapper handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }
}