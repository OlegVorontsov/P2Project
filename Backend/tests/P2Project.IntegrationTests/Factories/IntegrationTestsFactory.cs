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
using P2Project.Web;
using P2Project.Core.Files;
using P2Project.Core.Files.Models;
using P2Project.SharedKernel.Errors;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using FileInfo = P2Project.Core.Files.Models.FileInfo;

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
    private WriteDbContext _volunteersWriteDbContext;
    private Species.Infrastructure.DbContexts.WriteDbContext _speciesWriteDbContext;
    private IFileProvider _fileProvider = Substitute.For<IFileProvider>();
    private IMinioClient _minioClient = Substitute.For<IMinioClient>();
    private ILogger<MinioProvider> _logger = Substitute.For<ILogger<MinioProvider>>();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
    }
    
    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(IReadDbContext));
        services.RemoveAll(typeof(Species.Application.IReadDbContext));
        services.RemoveAll(typeof(WriteDbContext));
        services.RemoveAll(typeof(Species.Infrastructure.DbContexts.WriteDbContext));
        services.RemoveAll(typeof(IFileProvider));

        services.AddScoped(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped(_ =>
            new Species.Infrastructure.DbContexts.WriteDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<Species.Application.IReadDbContext, Species.Infrastructure.DbContexts.ReadDbContext>(_ =>
            new Species.Infrastructure.DbContexts.ReadDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IFileProvider, MinioProvider>(_ =>
            new MinioProvider(_minioClient, _logger));
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

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
            Arg.Any<Stream>(), Arg.Any<FileInfo>());
        
        _fileProvider.UploadFile(
                fileData, Arg.Any<CancellationToken>())
            .Returns(Errors.General.Failure("Интеграционный тест"));
    }
}