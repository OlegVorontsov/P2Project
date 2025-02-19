using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.AddMessageInDiscussionById;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.Close;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.EditMessage;
using P2Project.Discussions.Application.DiscussionsManagement.Commands.Reopen;
using P2Project.Discussions.Application.DiscussionsManagement.Queries;
using P2Project.Discussions.Application.DiscussionsManagement.Queries.GetById;
using P2Project.Discussions.Application.DiscussionsManagement.Queries.GetMessageById;
using P2Project.Discussions.Web.Requests;
using P2Project.Framework;
using P2Project.Framework.Authorization;

namespace P2Project.Discussions.Web;

[Authorize]
public class DiscussionsController : ApplicationController
{
    [Permission(PermissionsConfig.Discussions.Update)]
    [HttpPut("{userId:guid}/close/{discussionId:guid}")]
    public async Task<ActionResult> Close(
        [FromRoute] Guid userId,
        [FromRoute] Guid discussionId,
        [FromBody] CloseRequest request,
        [FromServices] CloseHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, discussionId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Update)]
    [HttpPut("{userId:guid}/reopen/{discussionId:guid}")]
    public async Task<ActionResult> Reopen(
        [FromRoute] Guid userId,
        [FromRoute] Guid discussionId,
        [FromBody] ReopenRequest request,
        [FromServices] ReopenHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, discussionId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Update)]
    [HttpPut("{userId:guid}/message/{discussionId:guid}")]
    public async Task<ActionResult> AddMessage(
        [FromRoute] Guid userId,
        [FromRoute] Guid discussionId,
        [FromBody] AddMessageRequest request,
        [FromServices] AddMessageHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, discussionId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Update)]
    [HttpPut("{userId:guid}/edit-message/{discussionId:guid}")]
    public async Task<ActionResult<Guid>> EditMessage(
        [FromRoute] Guid userId,
        [FromRoute] Guid discussionId,
        [FromBody] EditMessageRequest request,
        [FromServices] EditMessageHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, discussionId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Update)]
    [HttpPut("{userId:guid}/delete-message/{discussionId:guid}")]
    public async Task<ActionResult<Guid>> DeleteMessage(
        [FromRoute] Guid userId,
        [FromRoute] Guid discussionId,
        [FromBody] DeleteMessageRequest request,
        [FromServices] DeleteMessageHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(userId, discussionId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Read)]
    [HttpGet("{discussionId:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid discussionId,
        [FromServices] GetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(
            new GetByIdQuery(discussionId),
            cancellationToken);

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Discussions.Read)]
    [HttpGet("{messageId:guid}/message")]
    public async Task<IActionResult> GetMessageById(
        [FromRoute] Guid messageId,
        [FromServices] GetMessageByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(
            new GetMessageByIdQuery(messageId),
            cancellationToken);

        return Ok(result.Value);
    }
}