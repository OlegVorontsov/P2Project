using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Pets.CreateFile;
using P2Project.Application.Pets.DeleteFile;
using P2Project.Application.Pets.GetFile;

namespace P2Project.API.Controllers
{
    public class FileController : ApplicationController
    {
        [HttpPost]
        public async Task<IActionResult> CreateFile(
            IFormFile file,
            [FromServices] CreateFileHandler handler,
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFile(
            [FromRoute] Guid id,
            [FromServices] DeleteFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(new FileMetadata(
                "photos", id.ToString()), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetFile(
            [FromRoute] Guid id,
            [FromServices] GetFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(new FileMetadata(
                "photos", id.ToString()), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
