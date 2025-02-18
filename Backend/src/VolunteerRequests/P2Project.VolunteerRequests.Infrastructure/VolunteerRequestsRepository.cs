using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.SharedKernel.Errors;
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
    
    public async Task<Result<VolunteerRequest, Error>> GetById(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = await _dbContext.VolunteerRequests.
            SingleOrDefaultAsync(r => r.RequestId == requestId, cancellationToken);
        if (request is null)
            return Errors.General.NotFound(requestId);

        return request;
    }
}