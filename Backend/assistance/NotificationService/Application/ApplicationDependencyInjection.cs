using NotificationService.Application.EmailManagement.Send;
using NotificationService.Application.EventHandlers;
using NotificationService.Application.EveryDestinationManagement.Send;
using NotificationService.Application.Telegram.Send;
using NotificationService.Application.UserNotificationSettingsManagement.GetAny;
using NotificationService.Application.UserNotificationSettingsManagement.GetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.GetEmailSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetTelegramSendings;
using NotificationService.Application.UserNotificationSettingsManagement.GetWebSendings;
using NotificationService.Application.UserNotificationSettingsManagement.ResetByUserId;
using NotificationService.Application.UserNotificationSettingsManagement.SetByUserId;

namespace NotificationService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUserNotificationSettingsHandlers()
                .AddEveryDestinationHandlers()
                .AddEmailHandlers()
                .AddTelegramHandlers();
        
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
        services.AddScoped<SetByUserIdHandler>();
        services.AddScoped<ConfirmationEmailHandler>();

        return services;
    }
    
    private static IServiceCollection AddEveryDestinationHandlers(
        this IServiceCollection services)
    {
        services.AddScoped<SendEveryDestinationHandler>();

        return services;
    }
    
    private static IServiceCollection AddEmailHandlers(
        this IServiceCollection services)
    {
        services.AddScoped<SendEmailHandler>();

        return services;
    }
    
    private static IServiceCollection AddTelegramHandlers(
        this IServiceCollection services)
    {
        services.AddScoped<SendTelegramMessageHandler>();

        return services;
    }
}