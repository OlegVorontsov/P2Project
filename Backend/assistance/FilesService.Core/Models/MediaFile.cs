using CSharpFunctionalExtensions;
using FilesService.Core.Enums;
using FilesService.Core.ErrorManagment;

namespace FilesService.Core.Models;

public record MediaFile
{
    public Guid Key { get; }
    public FileType Type { get; }
    public string BucketName { get; }
    public string FileName { get; }
    public bool? IsMain { get; }

    private MediaFile() { }
    private MediaFile(
        FileType type,
        string bucketName,
        string fileName,
        bool? isMain)
    {
        Key = Guid.NewGuid();
        Type = type;
        BucketName = bucketName;
        FileName = fileName;
        IsMain = isMain;
    }

    public static Result<MediaFile, Error> Create(
        string bucketName,
        string fileName,
        bool? isMain,
        FileType type = FileType.Image)
    {
        if (string.IsNullOrWhiteSpace(bucketName)
            || string.IsNullOrWhiteSpace(fileName))
            return Errors.ValueIsInvalid("Название bucket и файла не должны быть пустыми");

        return new MediaFile(type, bucketName, fileName, isMain);
    }
}