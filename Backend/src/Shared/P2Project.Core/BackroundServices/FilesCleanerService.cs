using FilesService.Core.Dtos;
using FilesService.Core.Interfaces;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Services;

namespace P2Project.Core.BackroundServices;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;

    public FilesCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfoDto>> messageQueue)
    {
        _fileProvider = fileProvider;
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
