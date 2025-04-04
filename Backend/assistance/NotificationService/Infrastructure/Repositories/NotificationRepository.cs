using Microsoft.EntityFrameworkCore;
using NotificationService.Core.Dtos;
using NotificationService.Domain;
using NotificationService.Infrastructure.DbContexts;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository(NotificationWriteDbContext dbContext)
{
    public async Task<UserNotificationSettings?> Get(
        Guid userId, CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetAnySending(
        CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsEmailSend == true || n.IsWebSend == true || n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetEmailSendings(
        CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsEmailSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetTelegramSendings(
        CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsTelegramSend == true)
            .ToListAsync(ct);
        return getResult;
    }

    public async Task<IReadOnlyList<UserNotificationSettings>> GetWebSendings(
        CancellationToken ct)
    {
        var getResult = await dbContext.Notifications
            .Where(n => n.IsWebSend == true)
            .ToListAsync(ct);
        return getResult;
    }
    
    public async Task<UserNotificationSettings> Set(
        Guid userId,
        SentNotificationSettings notificationSettings,
        CancellationToken ct)
    {
        var userNotification = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        
        if (userNotification is null)
            return UserNotificationSettings.Create(userId);
        
        userNotification.Edit(
            notificationSettings.IsEmailSend,
            notificationSettings.IsTelegramSend,
            notificationSettings.IsWebSend);

        return userNotification;
    }

    public async Task Reset(Guid userId, CancellationToken ct)
    {
        var newNotificationSettings =
            new SentNotificationSettings(false, false, false);

        await Set(userId, newNotificationSettings, ct);
    }
}