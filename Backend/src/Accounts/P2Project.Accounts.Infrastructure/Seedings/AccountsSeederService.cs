using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.Accounts.Domain.Users.ValueObjects;
using P2Project.Accounts.Infrastructure.Admin;
using P2Project.Accounts.Infrastructure.Managers;
using P2Project.Accounts.Infrastructure.Permissions;
using P2Project.SharedKernel;

namespace P2Project.Accounts.Infrastructure.Seedings;

public class AccountsSeederService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    IOptions<AdminOptions> adminOptions,
    IAccountsManager accountsManager,
    ILogger<AccountsSeederService> logger)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;
    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync(
            Constants.ACCOUNTS_CONFIGURATIONS_FOLDER_PATH + Constants.ACCOUNTS_JSON_FILE_NAME);
        
        logger.LogInformation(json);
        
        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
            ?? throw new ApplicationException("RolePermissionConfig couldn't be deserialized");

        await SeedPermissions(seedData);
        await SeedRoles(seedData);
        await SeedRolePermissions(seedData);
        await SeedAdmin();
    }
    
    private async Task SeedPermissions(RolePermissionConfig seedData)
    {
        var permissionsToSeed = seedData.Permissions
            .SelectMany(group => group.Value);
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
            await rolePermissionManager
                .AddRangeIfDoesNotExist(role!.Id, seedData.Roles[roleName]);
        }
    }
    
    private async Task SeedAdmin()
    {
        var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
                        ?? throw new ApplicationException("Role admin couldn't be found");
        
        var adminFullName = FullName.Create(
            _adminOptions.UserName, _adminOptions.UserName, _adminOptions.UserName).Value;
        
        var adminUser = User.CreateAdmin(
            _adminOptions.Email, _adminOptions.UserName, adminFullName, adminRole);
        await userManager.CreateAsync(adminUser, _adminOptions.Password);

        var adminAccount = new AdminAccount(adminUser);
        await accountsManager.CreateAdminAccount(adminAccount);
        adminUser.AdminAccount = adminAccount;
        
        await userManager.UpdateAsync(adminUser);
    }
}