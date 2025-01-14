using P2Project.API.Middlewares;
using P2Project.Species.Web;
using P2Project.Volunteers.Web;
using P2Project.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
    .AddApi(config)
    .AddVolunteersModule(config)
    .AddSpeciesModule(config);

services.AddControllers();

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
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace P2Project.API
{
    public partial class Program;
}
