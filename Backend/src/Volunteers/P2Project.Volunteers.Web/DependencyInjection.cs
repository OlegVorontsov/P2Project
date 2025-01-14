using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Volunteers.Agreements;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Infrastructure;

namespace P2Project.Volunteers.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IPetsAgreement, PetsAgreement>()
            .AddVolunteersInfrastructure(configuration)
            .AddVolunteersApplication();
    }
}