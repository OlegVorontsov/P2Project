using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using P2Project.Infrastructure.Options;

namespace P2Project.API.Controllers
{
    public class FileController : ApplicationController
    {
        private readonly MinioOptions _minioOptions;
        public FileController(IOptions<MinioOptions> minioOptions)
        {
            _minioOptions = minioOptions.Value;
        }
        [HttpPost]
        public async Task<ActionResult> CreateFile()
        {
            return Ok(_minioOptions.EndPoint);
        }
    }
}
