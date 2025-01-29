using Microsoft.AspNetCore.Authorization;

namespace P2Project.Framework.Authorization;

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