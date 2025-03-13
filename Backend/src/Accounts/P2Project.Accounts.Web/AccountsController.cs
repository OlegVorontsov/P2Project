using FilesService.Core.Requests.AmazonS3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2Project.Accounts.Application.Commands.Login;
using P2Project.Accounts.Application.Commands.RefreshTokens;
using P2Project.Accounts.Application.Commands.Register;
using P2Project.Accounts.Application.Commands.SetAvatar.CompleteSetAvatar;
using P2Project.Accounts.Application.Commands.SetAvatar.UploadAvatar;
using P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;
using P2Project.Accounts.Web.Requests;
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
    
    //[Permission(PermissionsConfig.Files.Upload)]
    [HttpPost("upload-avatar")]
    public async Task<ActionResult> UploadAvatar(
        IFormFile avatarFile,
        [FromServices] UploadAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        var fileBytesArrayResult = await avatarFile.ToByteArrayAsync();
        if(fileBytesArrayResult.IsFailure)
            return fileBytesArrayResult.Error.ToResponse();
        
        var result = await handler.Handle(
            new UploadAvatarCommand(fileBytesArrayResult.Value,
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
    
    [HttpPost("complete-set-avatar")]
    public async Task<ActionResult> CompleteSetAvatar(
        [FromBody] CompleteSetAvatarRequest request,
        [FromServices] CompleteSetAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new CompleteSetAvatarCommand(
                    request.Key,
                    Constants.BUCKET_NAME_AVATARS,
                    request.UploadId,
                    request.ETag),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}