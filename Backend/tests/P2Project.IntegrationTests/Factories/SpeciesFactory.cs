using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using P2Project.IntegrationTests.Extensions;
using P2Project.Species.Application;
using P2Project.Species.Infrastructure.DbContexts;
using _Species = P2Project.Species.Domain.Species;

namespace P2Project.IntegrationTests.Factories;

public class SpeciesFactory :
    IClassFixture<IntegrationTestsFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsFactory _factory;
    protected readonly Fixture _fixture;
    protected readonly IServiceScope _scope;
    protected readonly SpeciesWriteDbContext _speciesWriteDbContext;
    protected readonly ISpeciesReadDbContext _speciesReadDbContext;
    private readonly SeedExtension _seedExtension;
    
    public SpeciesFactory(IntegrationTestsFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _speciesWriteDbContext = _scope
            .ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        _speciesReadDbContext = _scope
            .ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
        _seedExtension = new SeedExtension(factory);
    }
    
    protected async Task<_Species> SeedSpecies()
    {
        return await _seedExtension.SeedSpecies();
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