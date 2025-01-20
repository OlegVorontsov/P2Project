using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P2Project.Accounts.Application.Commands;
using P2Project.Accounts.Web.Requests;
using P2Project.Framework;

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

        return Ok(result);
    }
    
    [HttpPost("jwt")]
    public string Login(CancellationToken cancellationToken = default)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "userId"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ajnbpiusrtoibahiutbheatpihgpeiaughpiauhgpitugha"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddMinutes(10));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}