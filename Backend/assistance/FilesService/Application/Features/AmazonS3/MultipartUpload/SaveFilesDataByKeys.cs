using Amazon.S3;
using FilesService.Application.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses.AmazonS3;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class SaveFilesDataByKeys
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/save-data", Handler);
        }
    }
    private static async Task<IResult> Handler(
        [FromBody] SaveFilesDataByKeysRequest request,
        IAmazonS3 s3Client,
        IFilesRepository repository,
        CancellationToken cancellationToken)
    {
        try
        {
            List<FileLocationResponse> fileLocations = [];
            foreach (var fileRequest in request.FileRequestDtos)
            {
                var fileData = new FileData
                {
                    Id = Guid.NewGuid(),
                    StoragePath = fileRequest.FileKey.ToString(),
                    BucketName = fileRequest.BucketName,
                    UploadDate = DateTime.UtcNow,
                    FileSize = fileRequest.Lenght,
                    ContentType = fileRequest.ContentType
                };

                await repository.Add(fileData, cancellationToken);
                
                var fileLocation = new FileLocationResponse(
                    fileData.Id.ToString(), fileData.StoragePath);
                
                fileLocations.Add(fileLocation);
            }
            
            var response = new FilesSaveResponse(fileLocations);
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: complete multipart upload failed: \r\t\n{ex.Message}");
        }
    }
}