using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace P2Project.Accounts.Infrastructure.Permissions;

public class PermissionAttributeHandler : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute attribute)
    {
        var userId= context.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)!.Value;
        if (permission is null)
            return;
        
        if(permission.Value == attribute.Code)
            context.Succeed(attribute);
    }
}