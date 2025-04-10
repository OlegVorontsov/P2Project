using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.EmailManagement.Send;
using P2Project.Framework;

namespace NotificationService.Web.Controllers;

public class EmailController : BaseController
{
    [HttpPost("message")]
    public async Task<IActionResult> Send(
        [FromBody] SendCommand command,
        [FromServices] SendHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}