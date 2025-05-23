﻿using System.Data;

namespace P2Project.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<IDbTransaction> BeginTransaction(
            CancellationToken cancellationToken = default);

        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
