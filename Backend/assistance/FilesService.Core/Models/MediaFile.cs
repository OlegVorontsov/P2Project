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
        Guid fileKey,
        string fileName,
        bool? isMain)
    {
        Key = fileKey;
        Type = type;
        BucketName = bucketName;
        FileName = fileName;
        IsMain = isMain;
    }

    public static Result<MediaFile, Error> Create(
        string bucketName,
        string fileKey,
        string fileName,
        bool? isMain,
        FileType type = FileType.Image)
    {
        if (string.IsNullOrWhiteSpace(bucketName)
            || string.IsNullOrWhiteSpace(fileName))
            return Errors.ValueIsInvalid("Название bucket и файла не должны быть пустыми");

        if(!Guid.TryParse(fileKey, out Guid fileKeyGuid))
            return Errors.ValueIsInvalid("Не удалось создать fileKey");

        return new MediaFile(type, bucketName, fileKeyGuid, fileName, isMain);
    }
}