using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Files.DeleteFile;
using P2Project.Application.Files.GetFile;
using P2Project.Application.Files.UploadFile;
using P2Project.Application.Volunteers.Commands.AddPet;
using P2Project.Application.Volunteers.Commands.Create;
using P2Project.Application.Volunteers.Commands.Delete;
using P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.Commands.UpdateMainInfo;
using P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.Commands.UpdateSocialNetworks;
using P2Project.Application.Volunteers.Commands.UploadFilesToPet;
using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.Application.Shared
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<CreateHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdatePhoneNumbersHandler>();
            services.AddScoped<UpdateSocialNetworksHandler>();
            services.AddScoped<UpdateAssistanceDetailsHandler>();
            services.AddScoped<DeleteHandler>();
            services.AddScoped<GetPetsHandler>();

            services.AddScoped<UploadFileHandler>();
            services.AddScoped<DeleteFileHandler>();
            services.AddScoped<GetFileHandler>();

            services.AddScoped<Species.Create.CreateHandler>();
            services.AddScoped<Species.AddBreeds.AddBreedsHandler>();

            services.AddScoped<AddPetHandler>();
            services.AddScoped<UploadFilesToPetHandler>();
            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return services;
        }
    }
}
