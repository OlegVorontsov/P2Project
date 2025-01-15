using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Minio;
using Npgsql;
using NSubstitute;
using P2Project.API;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.SharedKernel.Errors;
using P2Project.Species.Application;
using P2Project.Species.Infrastructure.DbContexts;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;

namespace P2Project.IntegrationTests.Factories;

public class IntegrationTestsFactory :
    WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("P2Project_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    private Respawner _respawner;
    private DbConnection _dbConnection;
    private VolunteersWriteDbContext _volunteersWriteDbContext;
    private IFileProvider _fileProvider = Substitute.For<IFileProvider>();
    private IMinioClient _minioClient = Substitute.For<IMinioClient>();
    private ILogger<MinioProvider> _logger = Substitute.For<ILogger<MinioProvider>>();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
    }
    
    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(IVolunteersReadDbContext));
        services.RemoveAll(typeof(ISpeciesReadDbContext));
        services.RemoveAll(typeof(VolunteersWriteDbContext));
        services.RemoveAll(typeof(SpeciesWriteDbContext));
        services.RemoveAll(typeof(IFileProvider));

        services.AddScoped(_ =>
            new VolunteersWriteDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped(_ =>
            new SpeciesWriteDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
            new VolunteersReadDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
            new SpeciesReadDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IFileProvider, MinioProvider>(_ =>
            new MinioProvider(_minioClient, _logger));
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        
        await _volunteersWriteDbContext.Database.EnsureCreatedAsync();
        
        var speciesDbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        await speciesDbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
        await Task.CompletedTask;
    }   
    
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();

        await Task.CompletedTask;
    }
    
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
    
    public void SetupSuccessFileProvider()
    {
        var response = "test_file_name";
        
        _fileProvider.UploadFile(
                Arg.Any<FileData>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<string, Error>(response));
    }
    
    public void SetupFailureFileProvider()
    {
        var fileData = new FileData(
            Arg.Any<Stream>(), Arg.Any<FileInfoDto>());
        
        _fileProvider.UploadFile(
                fileData, Arg.Any<CancellationToken>())
            .Returns(Errors.General.Failure("Интеграционный тест"));
    }
}