using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.UserNotificationSettingsManagement.GetAny;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.GetEmailSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetTelegramSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetWebSendings;
using NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;
using NotificationService.Core.Dtos;

namespace NotificationService.Web.Controllers;

public class NotificationController : BaseController
{
    [HttpGet("notification-settings")]
    public async Task<IActionResult> GetAny(
        [FromServices] GetAnyHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(ct);
        return Ok(result);
    }

    [HttpGet("notification-settings/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(
        [FromRoute] Guid userId,
        [FromServices] GetByUserIdHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(userId, ct);
        return Ok(result);
    }

    [HttpGet("notification-settings/all-email-sendings")]
    public async Task<IActionResult> GetEmailSendings(
        [FromServices] GetEmailSendingsHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(ct);
        return Ok(result);
    }

    [HttpGet("notification-settings/all-telegram-sendings")]
    public async Task<IActionResult> GetTelegramSendings(
        [FromServices] GetTelegramSendingsHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(ct);
        return Ok(result);
    }

    [HttpGet("notification-settings/all-web-sendings")]
    public async Task<IActionResult> GetWebSendings(
        [FromServices] GetWebSendingsHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(ct);
        return Ok(result);
    }

    [HttpPost("notification-settings/{userId:guid}")]
    public async Task<IActionResult> ResetByUserId(
        [FromRoute] Guid userId,
        [FromServices] ResetByUserIdHandler handler,
        CancellationToken ct = default)
    {
        await handler.Handle(userId, ct);
        return Ok();
    }

    [HttpPost("notification-settings/new/{userId:guid}")]
    public async Task<IActionResult> SetByUserId(
        [FromRoute] Guid userId,
        [FromBody] SentNotificationSettings newNotificationSettings,
        [FromServices] SetByUserIdHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(userId, newNotificationSettings, ct);
        return Ok(result);
    }
}