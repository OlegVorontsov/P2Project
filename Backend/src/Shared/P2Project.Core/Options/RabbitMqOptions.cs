namespace P2Project.Core.Options;

public class RabbitMqOptions
{
    public const string SECTION_NAME = "RabbitMQ";
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}