namespace FilesService.Core.Dtos
{
    public record UploadFileDto(
        Stream Stream,
        string FileName,
        long Lenght,
        string ContentType);
}
