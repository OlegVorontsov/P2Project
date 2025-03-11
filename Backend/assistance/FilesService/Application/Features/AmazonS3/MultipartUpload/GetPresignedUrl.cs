using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Interfaces;
using FilesService.Core.Requests;
using FilesService.Core.Requests.AmazonS3;
using FilesService.Core.Responses;
using FilesService.Core.Responses.AmazonS3;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3.MultipartUpload;

public static class GetPresignedUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files/{key:guid}/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid key,
        [FromBody] GetPresignedUrlRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var keyString = key.ToString();
            
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = keyString,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddDays(14),
                Protocol = Protocol.HTTP
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            var response = new FileUrlResponse(keyString, presignedUrl); 
            return Results.Ok(response);
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3: get presigned url failed: \r\t\n{ex.Message}");
        }
    }
}