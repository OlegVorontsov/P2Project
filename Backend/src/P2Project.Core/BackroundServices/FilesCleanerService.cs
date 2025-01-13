using Microsoft.Extensions.Logging;
using P2Project.Core.Files;
using P2Project.Core.Interfaces.Services;
using P2Project.Core.Messaging;
using FileInfo = P2Project.Core.Files.Models.FileInfo;

namespace P2Project.Core.BackroundServices
{
    public partial class FilesCleanerBackgroundService
    {
        public class FilesCleanerService : IFilesCleanerService
        {
            private readonly IFileProvider _fileProvider;
            private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
            private readonly ILogger<FilesCleanerBackgroundService> _logger;

            public FilesCleanerService(
                IFileProvider fileProvider,
                ILogger<FilesCleanerBackgroundService> logger,
                IMessageQueue<IEnumerable<FileInfo>> messageQueue)
            {
                _fileProvider = fileProvider;
                _logger = logger;
                _messageQueue = messageQueue;
            }

            public async Task Process(CancellationToken cancellationToken)
            {
                var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

                foreach (var fileInfo in fileInfos)
                {
                    await _fileProvider.DeleteFileByFileInfo(fileInfo, cancellationToken);
                }
            }
        }
    }
}
