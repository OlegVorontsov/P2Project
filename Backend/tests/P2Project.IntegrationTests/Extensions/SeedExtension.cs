using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P2Project.IntegrationTests.Factories;
using P2Project.SharedKernel.IDs;
using P2Project.Species.Domain.Entities;
using P2Project.Species.Domain.ValueObjects;
using P2Project.UnitTestsFabrics;
using P2Project.Volunteers.Domain.Entities;
using P2Project.Volunteers.Domain.ValueObjects.Pets;
using P2Project.Volunteers.Infrastructure.DbContexts;
using _Species = P2Project.Species.Domain.Species;

namespace P2Project.IntegrationTests.Extensions;

public class SeedExtension
{
    protected readonly VolunteersWriteDbContext _writeDbContext;
    
    public SeedExtension(IntegrationTestsFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope
            .ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
    }
    
    public async Task<Guid> SeedVolunteer()
    {
        var newVolunteer = VolunteerFabric.CreateVolunteer();

        await _writeDbContext.AddAsync(newVolunteer);
        await _writeDbContext.SaveChangesAsync();
        
        var volunteer = _writeDbContext.Volunteers
            .FirstOrDefault(v => v.Id == newVolunteer.Id);
        if(volunteer == null)
            throw new Exception("Failed to create volunteer");

        return volunteer.Id.Value;
    }
    
    public async Task<_Species> SeedSpecies()
    {
        var breed = new Breed(BreedId.New(), Name.Create("test_breed_name").Value);
        
        var newSpecies = new _Species(
            SpeciesId.New(), Name.Create("test_species_name").Value,
            [breed]);

        await _writeDbContext.AddAsync(newSpecies);
        await _writeDbContext.SaveChangesAsync();

        var species = _writeDbContext.Species
            .FirstOrDefault(x => x.Id == newSpecies.Id);
        if(species == null)
            throw new Exception("Failed to create species");

        return species;
    }
    
    public async Task<Guid> SeedPet(Guid volunteerId)
    {
        var volunteer = await _writeDbContext.Volunteers
            .FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            throw new Exception($"Not found volunteer with Id:{volunteerId}");
        
        var species = _writeDbContext
            .Species.Include(s => s.Breeds).First();

        var pet = PetFabric.CreatePet(species.Id, species.Breeds.First().Id);

        volunteer.AddPet(pet);

        await _writeDbContext.SaveChangesAsync();
        
        return pet.Id.Value;
    }
    
    public async Task<Guid> SeedPetWithPhoto(Guid volunteerId)
    {
        var volunteer = await _writeDbContext.Volunteers
            .FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            return Guid.Empty;
        
        var species = _writeDbContext
            .Species.Include(s => s.Breeds).First();

        var pet = PetFabric.CreatePet(species.Id, species.Breeds.First().Id);
        
        var photos = new List<PetPhoto>{PetPhoto.Create("test_file_name.jpg", false).Value};
        
        pet.UpdatePhotos(photos);

        volunteer.AddPet(pet);

        await _writeDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
}