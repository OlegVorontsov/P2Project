using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Volunteers.CreateVolunteer;

namespace P2Project.Application.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<CreateVolunteerHandler>();
            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return services;
        }
    }
}
