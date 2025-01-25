using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Domain.User;
using P2Project.Accounts.Infrastructure.Admin;
using P2Project.Accounts.Infrastructure.Permissions;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.Seedings;

public class AccountsSeederService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeederService> logger)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;
    public async Task SeedRolesWithPermissions()
    {
        var json = await File.ReadAllTextAsync(
            Constants.ACCOUNTS_CONFIGURATIONS_FOLDER_PATH + Constants.ACCOUNTS_JSON_FILE_NAME);
        
        logger.LogInformation(json);
        
        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
            ?? throw new ApplicationException("RolePermissionConfig couldn't be deserialized");

        await SeedPermissions(seedData);
        await SeedRoles(seedData);
        await SeedRolePermissions(seedData);

        var adminUser = new User
        {
            UserName = _adminOptions.UserName,
            Email = _adminOptions.Email
        };
        await userManager.CreateAsync(adminUser, _adminOptions.Password);
    }
    
    private async Task SeedPermissions(RolePermissionConfig seedData)
    {
        var permissionsToSeed = seedData.Permissions.SelectMany(group => group.Value);
        await permissionManager.AddRangeIfDoesNotExist(permissionsToSeed);
    }
    
    private async Task SeedRoles(RolePermissionConfig seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var roleExist = await roleManager.FindByNameAsync(roleName);
            if (roleExist is null)
                await roleManager.CreateAsync(new Role { Name = roleName });
        }
    }

    private async Task SeedRolePermissions(RolePermissionConfig seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var rolePermissions = seedData.Roles[roleName];
            await rolePermissionManager.AddRangeIfDoesNotExist(role!.Id, seedData.Roles[roleName]);
        }
    }
}