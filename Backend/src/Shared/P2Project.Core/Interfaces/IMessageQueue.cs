namespace P2Project.Core.Interfaces
{
    public interface IMessageQueue<TMessage>
    {
        Task WriteAsync(
            TMessage message,
            CancellationToken cancellationToken);

        Task<TMessage> ReadAsync(
            CancellationToken cancellationToken);
    }
}
