using P2Project.Web.Middlewares;
using P2Project.Core.Extensions;
using P2Project.Species.Web;
using P2Project.Volunteers.Web;
using P2Project.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
    .AddApi(config)
    .AddVolunteersModule(config)
    .AddSpeciesModule(config);

services.AddControllers();

builder.Services.AddControllers();

var app = builder.Build();

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
    //await app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace P2Project.Web
{
    public partial class Program;
}
