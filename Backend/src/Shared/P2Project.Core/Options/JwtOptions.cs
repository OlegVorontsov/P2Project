namespace P2Project.Core.Options;

public class JwtOptions
{
    public const string NAME = "JWT";
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
    public int ExpiredMinute { get; init; }
}