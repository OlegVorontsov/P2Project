namespace P2Project.Core.Interfaces.Services
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancellationToken);
    }
}
