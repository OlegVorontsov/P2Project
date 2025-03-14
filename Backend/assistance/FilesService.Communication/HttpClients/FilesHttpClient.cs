using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;

namespace FilesService.Communication.HttpClients;

public class FilesHttpClient (HttpClient httpClient) : IFilesHttpClient
{
    public async Task<Result<IReadOnlyList<FileData>?, string>> GetFilesDataByIds(
        GetFilesDataByIdsRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("amazon/files", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Get files data by ids failed";

        var files = await response.Content.ReadFromJsonAsync<IReadOnlyList<FileData>>(ct);
        return files?.ToList() ?? [];
    }

    public async Task<Result<FileUrlResponse, string>> UploadPresignedUrl(
        UploadPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync("amazon/files/presigned", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Upload presigned url failed";

        var file = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return file ?? new FileUrlResponse("", "");
    }

    public async Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/complete-multipart/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Complete multipart upload failed";

        var fileLocation = await response.Content.ReadFromJsonAsync<FileLocationResponse>(ct);
        return fileLocation ?? new FileLocationResponse("", "");
    }

    public async Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Get presigned url failed";

        var fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl ?? new FileUrlResponse("", "");
    }

    public async Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
      StartMultipartUploadRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync("amazon/files/multipart/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Start multipart upload failed";

        var uploadResponse = await response.Content.ReadFromJsonAsync<UploadPartFileResponse>(ct);
        return uploadResponse ?? new UploadPartFileResponse("", "");
    }

    public async Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/part-presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Upload presigned part url failed";

        var fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl ?? new FileUrlResponse("", "");
    }
    
    public async Task<Result<string, Error>> UploadFileAsync(
        string url, byte[] file, string contentType, CancellationToken ct)
    {
        using var stream = new MemoryStream(file);

        using var content = new StreamContent(stream);
        
        var response = await httpClient.PutAsync(url, content, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return Errors.Failure("Upload file failed");

        if (!response.Headers.TryGetValues("ETag", out var etagValues))
            return Errors.Failure("Fail to get ETag");
        
        var eTag = etagValues.FirstOrDefault();
        if (eTag == null) return Errors.Failure("Fail to get ETag");
        
        return eTag.Trim('"');
    }
    
    public async Task<UnitResult<Error>> SaveFilesDataByKeys(
        SaveFilesDataByKeysRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync("amazon/files/save-data", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return Errors.Failure("Fail to save file data");
        
        return Result.Success<Error>();
    }
}