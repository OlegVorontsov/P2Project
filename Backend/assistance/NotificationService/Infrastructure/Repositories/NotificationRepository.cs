using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
using NotificationService.Domain.ValueObjects;
using NotificationService.Infrastructure.DbContexts;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository(NotificationWriteDbContext dbContext)
{
    public async Task Add(
        UserNotificationSettings userNotificationSettings, CancellationToken ct) =>
        await dbContext.Notifications.AddAsync(userNotificationSettings, ct);
    
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
            .Where(n =>
                n.IsEmailSend == true ||
                n.IsWebSend == true ||
                n.TelegramSettings != null)
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
            .Where(n => n.TelegramSettings != null)
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
    
    public async Task<TelegramSettings?> GetTelegramSettings(Guid userId, CancellationToken ct)
    {
        var notificationSettings = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.UserId == userId, ct);
        return notificationSettings?.TelegramSettings;
    }
}