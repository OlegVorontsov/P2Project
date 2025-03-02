using FilesService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FilesService.Application.Features;

public static class GetFilesDataByIds
{
    private record GetFilesDataByIdsRequest(IEnumerable<Guid> Ids);
    
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
        CancellationToken ct)
    {
        var result = await repository.GetRange(request.Ids, ct);
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest();
    }
}