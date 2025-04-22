namespace P2Project.Core.Interfaces.Outbox;

public interface IOutboxRepository
{
    Task Add<T>(T message, CancellationToken cancellationToken);
}