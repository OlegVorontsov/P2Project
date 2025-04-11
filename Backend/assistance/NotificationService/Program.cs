using NotificationService.Application;
using NotificationService.Core;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.TelegramNotification;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddInfrastructure(config)
    .AddApplication(config)
    .AddCore(config)
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("test", async (
    TelegramManager telegramManager) =>
{
    var userId = Guid.Parse("b38662d1-831f-4d0e-a2a7-2c52d2267d30");
    await telegramManager.StartRegisterChatId(userId);
    await telegramManager.SendMessage(userId, "test");
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
