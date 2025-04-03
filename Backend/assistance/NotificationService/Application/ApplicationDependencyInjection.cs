using NotificationService.Application.EmailManagement.Send;
using NotificationService.Application.UserNotificationSettingsManagement.GetAny;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.GetEmailSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetTelegramSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetWebSendings;
using NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.UpdateByUserId;

namespace NotificationService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUserNotificationSettingsHandlers()
                .AddEmailHandlers();
        
        return services;
    }
    
    private static IServiceCollection AddUserNotificationSettingsHandlers(
        this IServiceCollection services)
    {
        services.AddScoped<GetAnyHandler>();
        services.AddScoped<GetByUserIdHandler>();
        services.AddScoped<GetEmailSendingsHandler>();
        services.AddScoped<GetTelegramSendingsHandler>();
        services.AddScoped<GetWebSendingsHandler>();
        services.AddScoped<ResetByUserIdHandler>();
        services.AddScoped<UpdateByUserIdHandler>();

        return services;
    }
    
    private static IServiceCollection AddEmailHandlers(
        this IServiceCollection services)
    {
        services.AddScoped<SendHandler>();

        return services;
    }
}