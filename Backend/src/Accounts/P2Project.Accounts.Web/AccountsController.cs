using FilesService.Core.Requests.AmazonS3;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2Project.Accounts.Agreements.Messages;
using P2Project.Accounts.Application.Commands.EmailManagement.ConfirmEmail;
using P2Project.Accounts.Application.Commands.EmailManagement.GenerateEmailConfirmationToken;
using P2Project.Accounts.Application.Commands.Login;
using P2Project.Accounts.Application.Commands.RefreshTokens;
using P2Project.Accounts.Application.Commands.Register;
using P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;
using P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;
using P2Project.Accounts.Application.Commands.Unban;
using P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;
using P2Project.Accounts.Web.Requests;
using P2Project.Accounts.Web.Requests.EmailManagement;
using P2Project.Core.Extensions;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Web;

public class AccountsController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(), cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("confirmation-email/token/{userId:guid}")]
    public async Task<IActionResult> GenerateEmailConfirmation(
        [FromServices] GenerateEmailConfirmationTokenHandler generateEmailTokenHandler,
        [FromServices] GetUserInfoWithAccountsHandler getUserInfoHandler,
        [FromRoute] Guid userId,
        IPublishEndpoint publisher,
        CancellationToken ct)
    {
        var generateEmailTokenRequest = new GenerateEmailConfirmationTokenRequest(userId);
        var generateEmailTokenResult =
            await generateEmailTokenHandler.Handle(generateEmailTokenRequest, ct);

        var getUserInfoRequest = new GetUserInfoWithAccountsQuery(userId);
        var getUserInfoResult = 
            await getUserInfoHandler.Handle(getUserInfoRequest, ct);
        
        if (generateEmailTokenResult.IsFailure || getUserInfoResult.IsFailure)
            return BadRequest(generateEmailTokenResult.Error);

        var confirmRequest = new ConfirmEmailRequest(userId, generateEmailTokenResult.Value);
        
        var callbackUrl = Url.Action(
            nameof(ConfirmEmail),
            nameof(AccountsController).Replace("Controller", string.Empty),
            confirmRequest,
            protocol: HttpContext.Request.Scheme);
        
        var userDto = getUserInfoResult.Value;
        var createdUserEvent = new CreatedUserEvent(
            userDto.Id,
            userDto.Email,
            userDto.UserName,
            callbackUrl);
        await publisher.Publish(createdUserEvent, ct);

        return Ok(callbackUrl);
    }

    [HttpGet("confirmation-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromServices] ConfirmEmailHandler handler,
        [FromQuery] ConfirmEmailRequest confirmRequest,
        CancellationToken ct)
    {
        var result = await handler.Handle(confirmRequest, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Почта успешно подтверждена");
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(), cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshTokens(
        [FromBody] RefreshTokensRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(), cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Accounts.Update)]
    [HttpPut("unban/{userId:guid}")]
    public async Task<ActionResult> Unban(
        [FromRoute] Guid userId,
        [FromServices] UnbanHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new UnbanCommand(userId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Accounts.Read)]
    [HttpGet("user-info/{id:guid}")]
    public async Task<IActionResult> GetUserInfoWithAccounts(
        [FromRoute] Guid id,
        [FromServices] GetUserInfoWithAccountsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserInfoWithAccountsQuery(id);
        
        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Files.Upload)]
    [HttpPost("upload-avatar/{userId:guid}")]
    public async Task<ActionResult> UploadAvatar(
        [FromRoute] Guid userId,
        IFormFile avatarFile,
        [FromServices] UploadAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        var fileBytesArrayResult = await avatarFile.ToByteArrayAsync();
        if(fileBytesArrayResult.IsFailure)
            return fileBytesArrayResult.Error.ToResponse();
        
        var result = await handler.Handle(
            new UploadAvatarCommand(
                userId,
                fileBytesArrayResult.Value,
                new StartMultipartUploadRequest(
                    Constants.BUCKET_NAME_AVATARS,
                    avatarFile.FileName,
                    avatarFile.ContentType,
                    avatarFile.Length)),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(PermissionsConfig.Files.Upload)]
    [HttpPost("complete-set-avatar/{userId:guid}")]
    public async Task<ActionResult> CompleteSetAvatar(
        [FromRoute] Guid userId,
        [FromBody] CompleteSetAvatarRequest request,
        [FromServices] CompleteSetAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new CompleteSetAvatarCommand(
                    userId,
                    request.FileName,
                    Constants.BUCKET_NAME_AVATARS,
                    request.UploadId,
                    request.ETag),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}