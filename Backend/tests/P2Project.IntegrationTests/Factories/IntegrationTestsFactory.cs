using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using P2Project.Application.FileProvider;
using P2Project.Application.FileProvider.Models;
using P2Project.Application.Interfaces.DbContexts.Volunteers;
using P2Project.Domain.Shared.Errors;
using P2Project.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;
using FileInfo = P2Project.Application.FileProvider.Models.FileInfo;

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
    private IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();
    private WriteDbContext _writeDbContext; 
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefault);
    }
    
    private void ConfigureDefault(IServiceCollection services)
    {
        services.RemoveAll(typeof(IVolunteersReadDbContext));
        services.RemoveAll(typeof(WriteDbContext));
        services.RemoveAll(typeof(IFileProvider));

        services.AddScoped(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
            new VolunteersReadDbContext(_dbContainer.GetConnectionString()));
        services.AddTransient(_ => _fileProviderMock);
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
    
    public void SetupSuccessFileProviderMock()
    {
        var response = "test_file_name";
        
        var fileData = new FileData(
            Arg.Any<Stream>(), Arg.Any<FileInfo>());
        
        _fileProviderMock.UploadFile(
                fileData, CancellationToken.None)
            .Returns(Result.Success<string, Error>(response));
    }
    
    public void SetupFailureFileProviderMock()
    {
        var fileData = new FileData(
            Arg.Any<Stream>(), Arg.Any<FileInfo>());
        
        _fileProviderMock.UploadFile(
                fileData, CancellationToken.None)
            .Returns(Errors.General.Failure("Интеграционный тест"));
    }
}