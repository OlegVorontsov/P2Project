using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Response;

namespace P2Project.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApplicationController : ControllerBase
    { }
}
