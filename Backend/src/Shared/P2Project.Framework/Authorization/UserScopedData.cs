namespace P2Project.Framework.Authorization;

public class UserScopedData
{
    public Guid UserId { get; set; } = Guid.Empty;
    public List<string> Permissions { get; set; } = [];
    public List<string> Roles { get; set; } = [];
}