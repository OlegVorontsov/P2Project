using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

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
        context.Succeed(attribute);
    }
}