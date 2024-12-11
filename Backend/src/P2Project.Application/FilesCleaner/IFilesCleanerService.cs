namespace P2Project.Application.FilesCleaner
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancellationToken);
    }
}
