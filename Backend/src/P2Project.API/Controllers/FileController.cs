using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Files.CreateFile;
using P2Project.Application.Files.DeleteFile;
using P2Project.Application.Files.GetFile;

namespace P2Project.API.Controllers
{
    public class FileController : ApplicationController
    {
        public const string BUCKET_NAME_PHOTOS = "photos";
        [HttpPost]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromServices] UploadFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var result = await handler.Handle(new UploadFileRequest(
                stream,
                Guid.NewGuid().ToString(),
                BUCKET_NAME_PHOTOS), cancellationToken);

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
