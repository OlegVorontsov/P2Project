using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetApprovedStatus;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRejectStatus;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetReopenStatus;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByAdminId;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByUserId;
using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmitted;
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
    
    [Permission(PermissionsConfig.VolunteerRequests.Review)]
    [HttpPut("{adminId:guid}/reject/{requestId:guid}")]
    public async Task<ActionResult> SetRejectStatus(
        [FromRoute] Guid adminId,
        [FromRoute] Guid requestId,
        [FromServices] SetRejectStatusHandler handler,
        [FromBody] SetRejectStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(
            request.ToCommand(adminId, requestId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Review)]
    [HttpPut("{adminId:guid}/approve/{requestId:guid}")]
    public async Task<ActionResult> SetApprovedStatus(
        [FromRoute] Guid adminId,
        [FromRoute] Guid requestId,
        [FromBody] SetApprovedStatusRequest request,
        [FromServices] SetApprovedStatusHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(
            request.ToCommand(adminId, requestId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Update)]
    [HttpPut("{userId:guid}/reopen/{requestId:guid}")]
    public async Task<ActionResult> SetReopenStatus(
        [FromRoute] Guid userId,
        [FromRoute] Guid requestId,
        [FromBody] SetReopenStatusRequest request,
        [FromServices] SetReopenStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, requestId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Read)]
    [HttpGet("all-submitted")]
    public async Task<ActionResult> GetAllSubmitted(
        [FromQuery] GetAllSubmittedRequest request,
        [FromServices] GetAllSubmittedHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToQuery(), cancellationToken);
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Read)]
    [HttpGet("{adminId:guid}/all-by-admin")]
    public async Task<ActionResult> GetAllByAdminId(
        [FromRoute] Guid adminId,
        [FromQuery] GetAllByAdminIdRequest request,
        [FromServices] GetAllByAdminIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToQuery(adminId), cancellationToken);
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.VolunteerRequests.Read)]
    [HttpGet("{userId:guid}/all-by-user")]
    public async Task<ActionResult> GetAllByUserId(
        [FromRoute] Guid userId,
        [FromQuery] GetAllByUserIdRequest request,
        [FromServices] GetAllByUserIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToQuery(userId), cancellationToken);
        
        return Ok(result.Value);
    }
}