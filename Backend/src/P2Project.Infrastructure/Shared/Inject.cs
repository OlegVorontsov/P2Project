using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Volunteers;
using P2Project.Infrastructure.Interceptor;
using P2Project.Infrastructure.Repositories;

namespace P2Project.Infrastructure.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();
            services.AddSingleton<SoftDeleteInterceptor>();
            return services;
        }
    }
}
