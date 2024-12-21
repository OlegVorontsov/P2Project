using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.Shared.Models;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Extensions
{
    public static class QueriesExtensions
    {
        public static async Task<Result<PagedList<T>, Error>> ToPagedList<T>(
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

            if (items is null)
                return Errors.General.NotFound();

            return new PagedList<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = page,
                TotalCount = totalCount
            };
        }
        
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
