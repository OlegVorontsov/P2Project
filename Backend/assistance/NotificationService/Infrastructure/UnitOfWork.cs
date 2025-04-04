using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using NotificationService.Infrastructure.DbContexts;

namespace NotificationService.Infrastructure;

public class UnitOfWork(NotificationWriteDbContext dbContext)
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken ct)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(ct);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken ct) =>
        await dbContext.SaveChangesAsync(ct);
}