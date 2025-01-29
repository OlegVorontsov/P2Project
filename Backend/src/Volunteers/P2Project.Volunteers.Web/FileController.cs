using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2Project.Core.Dtos.Files;
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
        [HttpPost]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromServices] UploadFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var result = await handler.Handle(new UploadFileDto(
                stream, file.FileName), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Permission(PermissionsConfig.Files.Delete)]
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(
            string objectName,
            [FromServices] DeleteFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                objectName, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Permission(PermissionsConfig.Files.Read)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetFile(
            [FromRoute] Guid id,
            [FromServices] GetFileHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                id.ToString(), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
