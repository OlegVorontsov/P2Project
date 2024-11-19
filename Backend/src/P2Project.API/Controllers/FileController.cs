using Microsoft.AspNetCore.Mvc;
using Minio;
using P2Project.API.Extensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Pets.AddPhoto;
using P2Project.Application.Shared.Dtos;

namespace P2Project.API.Controllers
{
    public class FileController : ApplicationController
    {
        [HttpPost]
        public async Task<IActionResult> CreateFile(
            IFormFile file,
            [FromServices] AddPhotoHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var path = Guid.NewGuid().ToString();

            var result = await handler.Handle(new UploadFileRecord(
                stream,
                "photos",
                path), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
