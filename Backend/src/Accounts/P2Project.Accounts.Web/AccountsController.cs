using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2Project.Accounts.Application.Commands.Login;
using P2Project.Accounts.Application.Commands.Register;
using P2Project.Accounts.Infrastructure;
using P2Project.Accounts.Infrastructure.Permissions;
using P2Project.Accounts.Web.Requests;
using P2Project.Framework;

namespace P2Project.Accounts.Web;

public class AccountsController : ApplicationController
{
    [Permission("species.create")]
    [HttpGet("test-admin")]
    public ActionResult TestAdmin()
    {
        return Ok();
    }
    
    [Authorize]
    [HttpGet("test-user")]
    public ActionResult TestUser()
    {
        return Ok();
    }
    
    /*[HttpPost("registration")]
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
    }*/
    
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
}