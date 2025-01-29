using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace P2Project.Framework.Authorization;

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
        /*using var scope = _factory.CreateScope();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        
        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            context.Fail();
            return;
        }*/
        var permissions = context.User.Claims
            .Where(c => c.Type == CustomClaims.PERMISSION)
            .Select(c => c.Value)
            .ToList();
        
        if (permissions.Contains(attribute.Code))
        {
            context.Succeed(attribute);
            return;
        }
        context.Fail();
    }
}