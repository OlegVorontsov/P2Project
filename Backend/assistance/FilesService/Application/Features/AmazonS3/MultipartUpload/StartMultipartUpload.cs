using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Interfaces;
using FilesService.Core.Requests;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses;
using FilesService.Core.Responses.AmazonS3;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class StartMultipartUpload
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/multipart/presigned", Handler);
        }
    }
    private static async Task<IResult> Handler(
        [FromBody] StartMultipartUploadRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var key = Guid.NewGuid().ToString();

            var startMultipartRequest = new InitiateMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = key,
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = request.ContentType
                }
            };

            var response = await s3Client.InitiateMultipartUploadAsync(
                startMultipartRequest, cancellationToken);

            var uploadResponse = new UploadPartFileResponse(key, response.UploadId); 
            return Results.Ok(uploadResponse);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: start multipart upload failed: \r\t\n{ex.Message}");
        }
    }
}