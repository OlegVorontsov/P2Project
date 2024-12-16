namespace P2Project.Application.Interfaces.Services
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancellationToken);
    }
}
