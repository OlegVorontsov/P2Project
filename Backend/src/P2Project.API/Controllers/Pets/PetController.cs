using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Pets.Requests;
using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Controllers.Pets;

public class PetController : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();

        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }
}