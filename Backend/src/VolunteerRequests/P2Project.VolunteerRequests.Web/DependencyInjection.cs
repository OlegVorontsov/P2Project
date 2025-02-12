using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.VolunteerRequests.Application;
using P2Project.VolunteerRequests.Infrastructure;

namespace P2Project.VolunteerRequests.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddVolunteerRequestsInfrastructure(configuration)
            .AddVolunteerRequestsApplication();
    }
}