using Microsoft.AspNetCore.Mvc;
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
            var result = await handler.Handle(
                new CreateCommand(
                    request.FullName,
                    request.Age,
                    request.Gender,
                    request.Email,
                    request?.Description,
                    request.PhoneNumbers,
                    request?.SocialNetworks,
                    request?.AssistanceDetails), cancellationToken);

            return Ok(result.Value);
        }
    }
}
