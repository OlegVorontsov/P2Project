using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.EveryDestinationManagement.Send;

namespace NotificationService.Web.Controllers;

public class EveryDestinationController : BaseController
{
    [HttpPost("every-destination")]
    public async Task<IActionResult> Send(
        [FromServices] SendEveryDestinationHandler handler,
        [FromBody] SendEveryDestinationCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        
        return Ok(result);
    }
}