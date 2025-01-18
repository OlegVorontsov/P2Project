using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using P2Project.API.Middlewares;
using P2Project.Species.Web;
using P2Project.Volunteers.Web;
using P2Project.API;
using P2Project.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
    .AddApi(config)
    .AddVolunteersModule(config)
    .AddSpeciesModule(config);

services.AddControllers();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = "test",
            ValidAudience = "test",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ajnbpiusrtoibahiutbheatpihgpeiaughpiauhgpitugha")),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
        };
    });

services.AddAuthorization();

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
    await app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace P2Project.API
{
    public partial class Program;
}
