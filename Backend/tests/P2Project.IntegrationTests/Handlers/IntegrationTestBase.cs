using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Domain.Shared.IDs;
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
    protected readonly WriteDbContext _writeDbContext;

    public IntegrationTestBase(IntegrationTestsFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _volunteersReadDbContext = _scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
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
    
    protected async Task<Guid> SeedPet(Guid volunteerId)
    {
        var volunteer = await _writeDbContext.Volunteers.FindAsync(VolunteerId.Create(volunteerId));
        if (volunteer is null)
            return Guid.Empty;

        var pet = PetFabric.CreatePet();

        volunteer.AddPet(pet);

        await _writeDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
}