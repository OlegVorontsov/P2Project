namespace P2Project.Core.Options;

public class RefreshSessionOptions
{
    public const string REFRESH_SESSION = "RefreshSession";

    public string ExpiredDaysTime { get; init; } = string.Empty;
}