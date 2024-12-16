﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using P2Project.Application.Interfaces.Services;

namespace P2Project.Infrastructure.BackroundServices
{
    public partial class FilesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<FilesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FilesCleanerBackgroundService(
            ILogger<FilesCleanerBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Files cleaner background service started.");

            await using var scope = _scopeFactory.CreateAsyncScope();

            var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();

            while (!cancellationToken.IsCancellationRequested)
                await filesCleanerService.Process(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
