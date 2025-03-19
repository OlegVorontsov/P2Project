using CSharpFunctionalExtensions;
using P2Project.Accounts.Domain.RolePermission.Roles;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Agreements;

public interface IAccountsAgreements
{
    public Task<HashSet<string>> GetUserPermissionCodes(Guid userId);

    public Task<HashSet<Role>> GetUserRoles (Guid userId);
    
    public Task BanUser(Guid userId, CancellationToken cancellationToken);

    public Task<bool> IsUserBannedForVolunteerRequests(
        Guid userId, CancellationToken cancellationToken);
    
    public Task<Result<Guid, ErrorList>> CreateVolunteerAccountForUser(
        Guid userid, CancellationToken cancellationToken);
}