using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.Core.Interfaces;

namespace P2Project.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthorizationDbContext _dbContext;

    public UnitOfWork(AuthorizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(
        CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext
            .Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}