using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Telegram.Send;
using P2Project.Framework;

namespace NotificationService.Web.Controllers;

public class TelegramController : BaseController
{
    [HttpPost("telegram")]
    public async Task<IActionResult> Send(
        [FromServices] SendTelegramMessageHandler handler,
        [FromBody] SendTelegramMessageCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}