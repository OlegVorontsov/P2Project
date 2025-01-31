using Microsoft.Extensions.DependencyInjection;

namespace P2Project.Accounts.Infrastructure.Seedings;

public class AccountSeeder
{
    private readonly IServiceScopeFactory _factory;

    public AccountSeeder(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task SeedAsync()
    {
        using var scope = _factory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        await service.SeedAsync();
    }
}