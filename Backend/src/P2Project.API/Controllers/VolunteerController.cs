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
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request, cancellationToken);

            return Ok(result.Value);
        }
    }
}
