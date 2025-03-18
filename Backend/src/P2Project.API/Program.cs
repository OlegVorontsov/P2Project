using P2Project.Accounts.Infrastructure.Seedings;
using P2Project.Accounts.Web;
using P2Project.API.Middlewares;
using P2Project.Species.Web;
using P2Project.Volunteers.Web;
using P2Project.API;
using P2Project.API.Extensions;
using P2Project.Discussions.Web;
using P2Project.VolunteerRequests.Web;
using Serilog;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services
    .AddApi(config)
    .AddVolunteersModule(config)
    .AddSpeciesModule(config)
    .AddAccountsModule(config)
    .AddVolunteerRequestsModule(config)
    .AddDiscussionsModule(config)
    .AddHttpCommunications(config)
    .AddControllers();

var app = builder.Build();

var seeder = app.Services.GetRequiredService<AccountSeeder>();
await seeder.SeedAsync();

app.UseStaticFiles();
app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "P2Project.Api");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
    await app.ApplyMigrations();
}

app.UseCors(config =>
{
    config.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseScopeDataMiddleware();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace P2Project.API
{
    public partial class Program;
}
