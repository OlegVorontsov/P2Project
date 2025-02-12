using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Domain;
using P2Project.VolunteerRequests.Infrastructure.DbContexts;

namespace P2Project.VolunteerRequests.Infrastructure;

public class VolunteerRequestsRepository : IVolunteerRequestsRepository
{
    private readonly VolunteerRequestsWriteDbContext _dbContext;

    public VolunteerRequestsRepository(VolunteerRequestsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(
        VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.VolunteerRequests.AddAsync(volunteerRequest, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}