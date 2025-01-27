using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Accounts.Infrastructure.Models;

namespace P2Project.Accounts.Infrastructure.Permissions;

public class PermissionAttributeHandler :
    AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _factory;

    public PermissionAttributeHandler(IServiceScopeFactory factory)
    {
        _factory = factory;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute attribute)
    {
        using var scope = _factory.CreateScope();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        
        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            context.Fail();
            return;
        }
        var permissions = await permissionManager.GetUserPermissions(userId);
        if (permissions.Contains(attribute.Code))
        {
            context.Succeed(attribute);
            return;
        }
        context.Fail();
    }
}