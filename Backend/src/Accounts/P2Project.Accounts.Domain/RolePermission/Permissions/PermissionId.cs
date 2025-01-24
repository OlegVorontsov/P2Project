using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Domain.RolePermission.Permissions;

public record PermissionId
{
    public Guid Value { get; }
    private PermissionId(Guid value) => Value = value;

    public static Result<PermissionId, Error> Create(Guid value) =>
        new PermissionId(value);
    
    public static Result<PermissionId, Error> Create() =>
        new PermissionId(Guid.NewGuid());
}