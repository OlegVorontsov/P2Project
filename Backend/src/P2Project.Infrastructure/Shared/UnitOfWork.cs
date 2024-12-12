using Microsoft.EntityFrameworkCore.Storage;
using P2Project.Application.Shared;
using System.Data;

namespace P2Project.Infrastructure.Shared
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly WriteDBContext _dbContext;

        public UnitOfWork(WriteDBContext dbContext)
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
