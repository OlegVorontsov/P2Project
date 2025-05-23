using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using P2Project.Core.Interfaces;
using P2Project.Species.Infrastructure.DbContexts;

namespace P2Project.Species.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly SpeciesWriteDbContext _context;

    public UnitOfWork(SpeciesWriteDbContext context)
    {
        _context = context;
    }

    public async Task<IDbTransaction> BeginTransaction(
        CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}