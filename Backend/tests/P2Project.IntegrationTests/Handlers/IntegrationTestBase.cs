using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Core.Dtos.Pets;
using P2Project.IntegrationTests.Factories;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.Entities;
using P2Project.Species.Domain.ValueObjects;
using P2Project.UnitTestsFabrics;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Domain.ValueObjects.Pets;
using P2Project.Volunteers.Infrastructure.DbContexts;

namespace P2Project.IntegrationTests.Handlers;

public class IntegrationTestBase :
    IClassFixture<IntegrationTestsFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly IReadDbContext _volunteersReadDbContext;
    private readonly P2Project.Species.Application.IReadDbContext _speciesReadDbContext;
    protected readonly WriteDbContext _volunteersWriteDbContext;
    protected readonly P2Project.Species.Infrastructure.DbContexts.WriteDbContext _speciesWriteDbContext;
    
    public IntegrationTestBase(IntegrationTestsFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _volunteersReadDbContext = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        _speciesReadDbContext = _scope.ServiceProvider.GetRequiredService<P2Project.Species.Application.IReadDbContext>();
        _volunteersWriteDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _speciesWriteDbContext = _scope
            .ServiceProvider
            .GetRequiredService<P2Project.Species.Infrastructure.DbContexts.WriteDbContext>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
    
    protected async Task<Guid> SeedVolunteer()
    {
        var volunteer = VolunteerFabric.CreateVolunteer();

        await _volunteersWriteDbContext.AddAsync(volunteer);
        await _volunteersWriteDbContext.SaveChangesAsync();

        return volunteer.Id.Value;
    }
    
    protected async Task<SpeciesDto> SeedSpecies()
    {
        var breed = new Breed(BreedId.New(), Name.Create("test_breed_name").Value);
        
        var newSpecies = new P2Project.Species.Domain.Species(
            SpeciesId.New(), Name.Create("test_species_name").Value,
            [breed]);

        await _speciesWriteDbContext.AddAsync(newSpecies);
        await _speciesWriteDbContext.SaveChangesAsync();

        var species = _speciesReadDbContext
            .Species.Include(s => s.Breeds).First();

        return species;
    }
    
    protected async Task<Guid> SeedPet(Guid volunteerId)
    {
        var volunteer = await _volunteersWriteDbContext.Volunteers
            .FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            return Guid.Empty;
        
        var species = _speciesReadDbContext
            .Species.Include(s => s.Breeds).First();

        var pet = PetFabric.CreatePet(species.Id, species.Breeds.First().Id);

        volunteer.AddPet(pet);

        await _volunteersWriteDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
    
    protected async Task<Guid> SeedPetWithPhoto(Guid volunteerId)
    {
        var volunteer = await _volunteersWriteDbContext.Volunteers
            .FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            return Guid.Empty;
        
        var species = _speciesReadDbContext
            .Species.Include(s => s.Breeds).First();

        var pet = PetFabric.CreatePet(species.Id, species.Breeds.First().Id);
        
        var photos = new List<PetPhoto>{PetPhoto.Create("test_file_name.jpg", false).Value};
        
        pet.UpdatePhotos(photos);

        volunteer.AddPet(pet);

        await _volunteersWriteDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
}