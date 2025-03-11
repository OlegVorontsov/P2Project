using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests;
using FilesService.Core.Responses;

namespace FilesService.Communication.HttpClients;

public class AmazonFilesHttpClient (HttpClient httpClient) : IAmazonFilesHttpClient
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
        return file;
    }

    public async Task<Result<FileLocationResponse, string>> CompleteMultipartUpload(
        string key, CompleteMultipartRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/complete-multipart/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Complete multipart upload failed";

        var fileLocation = await response.Content.ReadFromJsonAsync<FileLocationResponse>(ct);
        return fileLocation;
    }

    public async Task<Result<FileUrlResponse, string>> GetPresignedUrl(
        string key, GetPresignedUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Get presigned url failed";

        var fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl;
    }

    public async Task<Result<UploadPartFileResponse, string>> StartMultipartUpload(
      StartMultipartUploadRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync("amazon/files/multipart/presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Start multipart upload failed";

        var uploadResponse = await response.Content.ReadFromJsonAsync<UploadPartFileResponse>(ct);
        return uploadResponse;
    }

    public async Task<Result<FileUrlResponse, string>> UploadPresignedPartUrl(
        string key, UploadPresignedPartUrlRequest request, CancellationToken ct)
    {
        var response = await httpClient
            .PostAsJsonAsync($"amazon/files/{key}/part-presigned", request, ct);
        
        if (response.StatusCode != HttpStatusCode.OK)
            return "Upload presigned part url failed";

        var fileUrl = await response.Content.ReadFromJsonAsync<FileUrlResponse>(ct);
        return fileUrl;
    }
}