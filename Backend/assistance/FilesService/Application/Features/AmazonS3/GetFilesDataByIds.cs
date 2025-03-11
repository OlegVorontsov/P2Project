using Amazon.S3;
using FilesService.Application.Interfaces;
using FilesService.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features.AmazonS3;

public static class GetFilesDataByIds
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("amazon/files", Handler);
        }
    }
    private static async Task<IResult> Handler(
        [FromBody] GetFilesDataByIdsRequest request,
        IFilesRepository repository,
        IAmazonS3 s3Client,
        CancellationToken ct)
    {
        var result = await repository.GetRange(request.Ids, ct);
        
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest();
    }
}