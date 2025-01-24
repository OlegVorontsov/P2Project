using Microsoft.AspNetCore.Authorization;

namespace P2Project.Accounts.Infrastructure;

public class PermissionAttribute :
    AuthorizeAttribute, IAuthorizationRequirement
{
    public PermissionAttribute(string code) :
        base(policy: code)
    {
        Code = code;
    }
    public string Code { get; private set; }
}