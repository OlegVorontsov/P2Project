﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Pets.CreateFile;
using P2Project.Application.Pets.DeleteFile;
using P2Project.Application.Pets.GetFile;
using P2Project.Application.Volunteers.CreatePet;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Application.Volunteers.Delete;
using P2Project.Application.Volunteers.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.UpdateMainInfo;
using P2Project.Application.Volunteers.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.UpdateSocialNetworks;

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
            services.AddScoped<UploadFileHandler>();
            services.AddScoped<DeleteFileHandler>();
            services.AddScoped<GetFileHandler>();
            services.AddScoped<Species.Create.CreateHandler>();
            services.AddScoped<Species.AddBreeds.AddBreedsHandler>();
            services.AddScoped<CreatePetHandler>();
            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
            return services;
        }
    }
}
