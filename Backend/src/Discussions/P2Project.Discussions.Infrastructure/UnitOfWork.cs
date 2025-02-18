using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using P2Project.Core.Interfaces;
using P2Project.Discussions.Infrastructure.DbContexts;

namespace P2Project.Discussions.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public UnitOfWork(DiscussionsWriteDbContext dbContext)
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