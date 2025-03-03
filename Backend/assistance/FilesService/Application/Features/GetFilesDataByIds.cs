using Amazon.S3;
using Amazon.S3.Model;
using FilesService.Application.Interfaces;
using FilesService.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features;

public static class GetFilesDataByIds
{
    private record GetFilesDataByIdsRequest(IEnumerable<Guid> Ids, string BucketName);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files", Handler);
        }
    }
    private static async Task<IResult> Handler(
        [FromBody] GetFilesDataByIdsRequest request,
        IFilesRepository repository,
        IAmazonS3 s3Client,
        CancellationToken ct)
    {
        var result = await repository.GetRange(request.Ids, ct);
        
        ICollection<FileDataResponse> response = [];
        if (result.IsSuccess)
        {
            foreach (var fileData in result.Value)
            {
                try
                {
                    var keyString = fileData.StoragePath;
            
                    var presignedRequest = new GetPreSignedUrlRequest
                    {
                        BucketName = fileData.BucketName,
                        Key = keyString,
                        Verb = HttpVerb.GET,
                        Expires = DateTime.UtcNow.AddDays(14),
                        Protocol = Protocol.HTTP
                    };

                    var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

                    response.Add(new FileDataResponse(
                        keyString, presignedUrl, fileData.UploadDate, fileData.FileSize, fileData.ContentType));
                }
                catch (AmazonS3Exception ex)
                {
                    return Results.BadRequest($"S3: get presigned url failed: \r\t\n{ex.Message}");
                }
            }
            return Results.Ok(response);
        }
        
        return Results.BadRequest();
    }
}