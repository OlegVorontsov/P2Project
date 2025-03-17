using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;

namespace FilesService.Core.Interfaces;

public interface IFilesHttpClient
{
    public Task<Result<IReadOnlyList<FileData>?, string>> GetFilesDataByIds(
        GetFilesDataByIdsRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct);

    public Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct);
    
    public Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
        StartMultipartUploadRequest request, CancellationToken ct);
    
    public Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct);

    public Task<Result<string, Error>> UploadFileAsync(
        string url, byte[] file, string contentType, CancellationToken ct);

    public Task<Result<List<FileLocationResponse>, Error>> SaveFilesDataByKeys(
        SaveFilesDataByKeysRequest request, CancellationToken ct);
}