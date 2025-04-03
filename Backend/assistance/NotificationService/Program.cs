using NotificationService.Application;
using NotificationService.Core;
using NotificationService.Infrastructure;

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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
