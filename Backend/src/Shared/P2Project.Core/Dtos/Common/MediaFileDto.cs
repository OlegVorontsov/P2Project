namespace P2Project.Core.Dtos.Common;

public record MediaFileDto(
    string BucketName,
    string FileKey,
    string FileName,
    bool? IsMain);