using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;
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
    
    [Permission(PermissionsConfig.VolunteerRequests.Review)]
    [HttpPut("{adminId:guid}/take-in-review")]
    public async Task<ActionResult> TakeInReview(
        [FromRoute] Guid adminId,
        [FromBody] TakeInReviewRequest request,
        [FromServices] TakeInReviewHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(adminId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Update)]
    [HttpPut("{adminId:guid}/revision-required/{requestId:guid}")]
    public async Task<ActionResult> SetRevisionRequiredStatus(
        [FromRoute] Guid adminId,
        [FromRoute] Guid requestId,
        [FromBody] SetRevisionRequiredStatusRequest request,
        [FromServices] SetRevisionRequiredStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(adminId, requestId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}