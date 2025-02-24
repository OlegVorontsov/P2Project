using Microsoft.AspNetCore.Mvc;
using P2Project.Core.Models;

namespace P2Project.Framework
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApplicationController : ControllerBase
    {
        public override OkObjectResult Ok(object? value)
        {
            return base.Ok(Envelope.Ok(value));
        }
    }
}
