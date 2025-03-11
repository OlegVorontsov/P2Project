using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FilesService.Core.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.Minio;

namespace FilesService.Communication.HttpClients;

public class MinioFilesHttpClient(HttpClient httpClient) : IMinioFilesHttpClient
{
    public async Task<Result<MediaFile, string>> UploadFile(
        UploadFileRequest request, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync("minio/file-presigned-path", request, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return "Failed to upload the file";

        var result = await response.Content.ReadFromJsonAsync<MediaFile>(ct);
        return result;
    }
}