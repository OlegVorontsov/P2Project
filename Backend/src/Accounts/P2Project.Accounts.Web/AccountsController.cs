using Microsoft.AspNetCore.Mvc;
using P2Project.Accounts.Application.Commands.Login;
using P2Project.Accounts.Application.Commands.RefreshTokens;
using P2Project.Accounts.Application.Commands.Register;
using P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;
using P2Project.Accounts.Web.Requests;
using P2Project.Framework;
using P2Project.Framework.Authorization;

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
}