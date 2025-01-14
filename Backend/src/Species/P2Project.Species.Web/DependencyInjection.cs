using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Species.Agreements;
using P2Project.Species.Application;
using P2Project.Species.Infrastructure;

namespace P2Project.Species.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<ISpeciesAgreement, SpeciesAgreement>()
            .AddSpeciesInfrastructure(configuration)
            .AddSpeciesApplication();
    }
}