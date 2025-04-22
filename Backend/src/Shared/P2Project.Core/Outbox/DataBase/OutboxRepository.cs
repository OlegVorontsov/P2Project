using System.Text.Json;
using P2Project.Core.Interfaces.Outbox;
using P2Project.Core.Outbox.Models;

namespace P2Project.Core.Outbox.DataBase;

public class OutboxRepository(
    OutboxDbContext dbContext) : IOutboxRepository
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
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}