using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands;
using P2Project.VolunteerRequests.Web.Requests;

namespace P2Project.VolunteerRequests.Web;

[Authorize]
public class VolunteerRequestsController : ApplicationController
{
    [Permission(PermissionsConfig.VolunteerRequests.Create)]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] CreateVolunteerRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}