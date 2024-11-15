using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using P2Project.Domain.Shared;

namespace P2Project.Infrastructure.Interceptor
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public async override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if(eventData.Context is null)
                return await base.SavedChangesAsync(eventData, result, cancellationToken);

            var entries = eventData.Context.ChangeTracker
                .Entries()
                .Where(v => v.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                if (entry.Entity is ISoftDeletable item)
                    item.Deleted();
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
