using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Discussions.Application;
using P2Project.Discussions.Infrastructure;

namespace P2Project.Discussions.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDiscussionsInfrastructure(configuration)
            .AddDiscussionsApplication();
    }
}