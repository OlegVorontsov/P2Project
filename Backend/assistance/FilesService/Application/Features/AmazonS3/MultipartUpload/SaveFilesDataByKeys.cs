using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Interfaces;
using FilesService.Core.Models;
using FilesService.Core.Requests.AmazonS3;
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
            var metaDataRequests = request.FilePaths
                .Select(f => new GetObjectMetadataRequest
                {
                    BucketName = request.BucketName,
                    Key = f.ToString()
                });

            foreach (var dataRequest in metaDataRequests)
            {
                //var metaData = await s3Client.GetObjectMetadataAsync(dataRequest, cancellationToken);

                var fileData = new FileData
                {
                    Id = Guid.NewGuid(),
                    StoragePath = dataRequest.Key,
                    BucketName = dataRequest.BucketName,
                    UploadDate = DateTime.UtcNow,
                    FileSize = 10000,
                    ContentType = request.ContentType
                };

                await repository.Add(fileData, cancellationToken);
            }
            
            return Results.Ok();
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: complete multipart upload failed: \r\t\n{ex.Message}");
        }
    }
}