using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using P2Project.Core.Files;
using P2Project.IntegrationTests.Handlers;

namespace P2Project.IntegrationTests.Factories;

public class FileProviderFactory : IntegrationTestBase
{    
    protected readonly IFileProvider _fileProvider = Substitute.For<IFileProvider>();
    private static IntegrationTestsFactory _factory;
    protected readonly IServiceScope _scope;
    
    public FileProviderFactory(IntegrationTestsFactory factory) : base(factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _fileProvider = _scope.ServiceProvider.GetRequiredService<IFileProvider>();
        _factory.SetupSuccessFileProvider();
    }
    
    public static void SetFileProviderSuccess() => _factory.SetupSuccessFileProvider();
    public static void SetFileProviderFailed() => _factory.SetupFailureFileProvider();
}