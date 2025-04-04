using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.EmailManagement.Send;

namespace NotificationService.Web.Controllers;

public class EmailController : BaseController
{
    [HttpPost("message")]
    public async Task<IActionResult> Send(
        [FromBody] SendCommand command,
        [FromServices] SendHandler handler,
        CancellationToken ct)
    {
        await handler.Handle(command, ct);
        return Ok();
    }
}