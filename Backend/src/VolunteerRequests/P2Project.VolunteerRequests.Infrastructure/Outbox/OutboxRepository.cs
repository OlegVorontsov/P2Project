using System.Text.Json;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Infrastructure.DbContexts;

namespace P2Project.VolunteerRequests.Infrastructure.Outbox;

public class OutboxRepository(
    VolunteerRequestsWriteDbContext dbContext) : IOutboxRepository
{
    public async Task Add<T>(T message, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.Now,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message),
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}