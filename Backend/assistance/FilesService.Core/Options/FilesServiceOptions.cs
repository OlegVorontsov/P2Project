namespace FilesService.Core.Options;

public class FilesServiceOptions
{
    public const string SECTION_NAME = "FilesService";
    public string Url { get; init; } = string.Empty;
}