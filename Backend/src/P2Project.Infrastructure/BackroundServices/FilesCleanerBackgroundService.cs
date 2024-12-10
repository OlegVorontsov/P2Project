using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace P2Project.Infrastructure.BackroundServices
{
    public class FilesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<FilesCleanerBackgroundService> _logger;

        public FilesCleanerBackgroundService(
            ILogger<FilesCleanerBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("working...");
                await Task.Delay(3000, stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}
