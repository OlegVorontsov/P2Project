using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Interfaces;
using FilesService.Core.Responses;

namespace FilesService.Application.Features.AmazonS3;

public static class GetPresignedUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        Guid key,
        string bucket,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var keyString = key.ToString();
            
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
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