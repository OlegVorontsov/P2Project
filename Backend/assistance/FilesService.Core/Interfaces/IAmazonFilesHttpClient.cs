using CSharpFunctionalExtensions;
using FilesService.Core.Models;
using FilesService.Core.Requests;
using FilesService.Core.Responses;

namespace FilesService.Core.Interfaces;

public interface IAmazonFilesHttpClient
{
    public Task<Result<IReadOnlyList<FileData>?, string>> GetFilesDataByIds(
        GetFilesDataByIdsRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct);

    public Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        Guid key, GetPresignedUrlRequest request, CancellationToken ct);
    
    public Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
        StartMultipartUploadRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct);
}