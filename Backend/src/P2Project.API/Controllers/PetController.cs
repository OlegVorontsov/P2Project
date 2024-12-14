using Microsoft.AspNetCore.Mvc;
using P2Project.API.Requests.Pets;
using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Controllers;

public class PetController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }
}