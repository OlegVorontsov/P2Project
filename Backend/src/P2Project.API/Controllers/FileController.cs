using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;

namespace P2Project.API.Controllers
{
    public class FileController : ApplicationController
    {
        [HttpPost]
        public async Task<IActionResult> Create(
            IFormFile file,
            [FromServices] IFileProvider provider,
            CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();

            var uploadFileRecord = new UploadFileRecord(
                stream,
                "photos",
                "");

            var result = await provider.UploadFile(
                uploadFileRecord,
                cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
