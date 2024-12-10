using System.Threading.Channels;

namespace P2Project.Infrastructure.MessageQueues
{
    public class FilesCleaner
    {
        private readonly Channel<string[]> _channel = Channel.CreateUnbounded<string[]>();

        public async Task WriteAsync(
            string[] paths,
            CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(paths, cancellationToken);
        }

        public async Task ReadAsync(
            CancellationToken cancellationToken = default)
        {
            await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
