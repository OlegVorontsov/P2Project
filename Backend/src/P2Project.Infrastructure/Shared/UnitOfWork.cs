using Microsoft.EntityFrameworkCore.Storage;
using P2Project.Application.Shared;
using System.Data;
using P2Project.Application.Interfaces;
using P2Project.Infrastructure.DbContexts;

namespace P2Project.Infrastructure.Shared
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly WriteDbContext _dbContext;

        public UnitOfWork(WriteDbContext dbContext)
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
}
