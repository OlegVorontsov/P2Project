﻿using Microsoft.EntityFrameworkCore;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<PagedList<T>> ToPagedList<T>(
            this IQueryable<T> source,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);

            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = page,
                TotalCount = totalCount
            };
        }
    }
}