using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NSubstitute;
using P2Project.Application.Interfaces.DbContexts.Species;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Domain.Shared.IDs;
using P2Project.Domain.SpeciesManagment;
using P2Project.Domain.SpeciesManagment.Entities;
using P2Project.Domain.SpeciesManagment.ValueObjects;
using P2Project.Infrastructure.DbContexts;
using P2Project.IntegrationTests.Factories;
using P2Project.UnitTestsFabrics;

namespace P2Project.IntegrationTests.Handlers;

public class IntegrationTestBase :
    IClassFixture<IntegrationTestsFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ISpeciesReadDbContext _speciesReadDbContext;
    protected readonly WriteDbContext _writeDbContext;
    
    public IntegrationTestBase(IntegrationTestsFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _volunteersReadDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
        _speciesReadDbContext = _scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
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

        await _writeDbContext.AddAsync(volunteer);
        await _writeDbContext.SaveChangesAsync();

        return volunteer.Id.Value;
    }
    
    protected async Task<SpeciesDto> SeedSpecies()
    {
        var breed = new Breed(BreedId.New(), Name.Create("test_breed_name").Value);
        
        var newSpecies = new Species(
            SpeciesId.New(), Name.Create("test_species_name").Value,
            [breed]);

        await _writeDbContext.AddAsync(newSpecies);
        await _writeDbContext.SaveChangesAsync();

        var species = _speciesReadDbContext
            .Species.Include(s => s.Breeds).First();

        return species;
    }
    
    protected async Task<Guid> SeedPet(Guid volunteerId)
    {
        var volunteer = await _writeDbContext.Volunteers
            .FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            return Guid.Empty;
        
        var species = _speciesReadDbContext
            .Species.Include(s => s.Breeds).First();

        var pet = PetFabric.CreatePet(species.Id, species.Breeds.First().Id);

        volunteer.AddPet(pet);

        await _writeDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
}