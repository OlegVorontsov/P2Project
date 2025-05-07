using System.Data.Common;
using CSharpFunctionalExtensions;
using FilesService.Communication;
using FilesService.Core.Dtos;
using FilesService.Core.ErrorManagment;
using FilesService.Core.Interfaces;
using FilesService.Core.Requests.Minio;
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
using P2Project.Species.Application;
using P2Project.Species.Infrastructure.DbContexts;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Interfaces;
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
        services.RemoveAll(typeof(VolunteersWriteDbContext));
        services.RemoveAll(typeof(SpeciesWriteDbContext));
        services.RemoveAll(typeof(IVolunteersReadDbContext));
        services.RemoveAll(typeof(ISpeciesReadDbContext));
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
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _volunteersWriteDbContext = Services
            .CreateScope().ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        await _volunteersWriteDbContext.Database.EnsureCreatedAsync();
        
        await InitializeRespawner();
    }
    
    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
    
    public async Task ResetDatabaseAsync()
    {
        if (_respawner is not null)
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
                Arg.Any<UploadFileRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<string, Error>(response));
    }
    
    public void SetupFailureFileProvider()
    {
        var uploadFileRequest = new UploadFileRequest(
            Arg.Any<Stream>(), Arg.Any<FileInfoDto>());
        
        _fileProvider.UploadFile(
                uploadFileRequest, Arg.Any<CancellationToken>())
            .Returns(Errors.Failure("Интеграционный тест"));
    }
}