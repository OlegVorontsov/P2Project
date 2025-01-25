using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Infrastructure.Permissions;
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
            Constants.ACCOUNTS_CONFIGURATIONS_FOLDER_PATH + Constants.ACCOUNTS_JSON_FILE_NAME);
        
        _logger.LogInformation(json);
        
        using var scope = _factory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        var rolePermissionManager = scope.ServiceProvider.GetRequiredService<RolePermissionManager>();
        
        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
            ?? throw new ApplicationException("RolePermissionConfig couldn't be deserialized");

        await SeedPermissions(seedData, permissionManager);
        await SeedRoles(seedData, roleManager);
        await SeedRolePermissions(seedData, roleManager, rolePermissionManager);
    }
    
    private async Task SeedPermissions(
        RolePermissionConfig seedData, PermissionManager permissionManager)
    {
        var permissionsToSeed = seedData.Permissions.SelectMany(group => group.Value);
        await permissionManager.AddRangeIfDoesNotExist(permissionsToSeed);
    }
    
    private async Task SeedRoles(
        RolePermissionConfig seedData, RoleManager<Role> roleManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var roleExist = await roleManager.FindByNameAsync(roleName);
            if (roleExist is null)
                await roleManager.CreateAsync(new Role { Name = roleName });
        }
    }

    private async Task SeedRolePermissions(RolePermissionConfig seedData, RoleManager<Role> roleManager,
        RolePermissionManager rolePermissionManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var rolePermissions = seedData.Roles[roleName];
            await rolePermissionManager.AddRangeIfDoesNotExist(role!.Id, seedData.Roles[roleName]);
        }
    }
}

public class RolePermissionConfig
{
    public Dictionary<string, string[]> Permissions { get; set; } = [];
    public Dictionary<string, string[]> Roles { get; set; } = [];

}