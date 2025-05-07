using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using P2Project.IntegrationTests.Extensions;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Infrastructure.DbContexts;
using _Species = P2Project.Species.Domain.Species;

namespace P2Project.IntegrationTests.Factories;

public class VolunteerFactory :
    IClassFixture<IntegrationTestsFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly VolunteersWriteDbContext _volunteersWriteDbContext;
    protected readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly SeedExtension _seedExtension;
    
    public VolunteerFactory(IntegrationTestsFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _volunteersWriteDbContext = _scope
            .ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        _volunteersReadDbContext = _scope
            .ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
        _seedExtension = new SeedExtension(factory);
    }
    
    protected async Task<Guid> SeedVolunteer()
    {
        return await _seedExtension.SeedVolunteer();
    }
    
    protected async Task<(Guid VolunteerId, _Species Species)> SeedVolunteerAndSpecies()
    {
        var volunteerId = await _seedExtension.SeedVolunteer();
        var species = await _seedExtension.SeedSpecies();
        return (volunteerId, species);
    }
    
    protected async Task<Guid> SeedPet(Guid volunteerId)
    {
        return await _seedExtension.SeedPet(volunteerId);
    }
    
    protected async Task<Guid> SeedPetWithPhoto(Guid volunteerId)
    {
        return await _seedExtension.SeedPetWithPhoto(volunteerId);
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
}