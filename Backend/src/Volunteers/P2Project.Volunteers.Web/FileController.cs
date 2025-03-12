using FilesService.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2Project.Framework;
using P2Project.Framework.Authorization;
using P2Project.Volunteers.Application.Files.DeleteFile;
using P2Project.Volunteers.Application.Files.GetFile;
using P2Project.Volunteers.Application.Files.UploadFile;

namespace P2Project.Volunteers.Web
{
    [Authorize]
    public class FileController : ApplicationController
    {
        [Permission(PermissionsConfig.Files.Upload)]
        [HttpPost("{bucketName}")]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromRoute] string bucketName,
            [FromServices] UploadFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var result = await handler.Handle(
                new UploadFileDto(stream, file.FileName),
                bucketName,
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Permission(PermissionsConfig.Files.Delete)]
        [HttpDelete("{bucketName}/file/{fileName}")]
        public async Task<IActionResult> DeleteFile(
            [FromRoute] string bucketName,
            [FromRoute] string fileName,
            [FromServices] DeleteFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                bucketName, fileName, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Permission(PermissionsConfig.Files.Read)]
        [HttpGet("{bucketName}/file/{fileName}")]
        public async Task<IActionResult> GetFile(
            [FromRoute] string bucketName,
            [FromRoute] string fileName,
            [FromServices] GetFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                bucketName, fileName, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
