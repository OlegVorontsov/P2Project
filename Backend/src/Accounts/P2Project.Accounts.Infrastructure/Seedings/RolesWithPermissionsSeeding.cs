using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.Seedings;

public class RolesWithPermissionsSeeding
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<RolesWithPermissionsSeeding> _logger;

    public RolesWithPermissionsSeeding(
        IServiceScopeFactory factory,
        ILogger<RolesWithPermissionsSeeding> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task SeedRolesWithPermissions()
    {
        var json = await File.ReadAllTextAsync(
            Constants.CONFIGURATIONS_FOLDER_PATH + Constants.ROLES_JSON_FILE_NAME);
        
        _logger.LogInformation(json);
        
        using var scope = _factory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    }
}

public class RolePermissionConfig
{
    public Dictionary<string, string[]> Permissions { get; set; } = [];
    public Dictionary<string, string[]> Roles { get; set; } = [];

}