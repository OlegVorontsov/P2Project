using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Domain;

namespace P2Project.VolunteerRequests.Application.Interfaces;

public interface IVolunteerRequestsRepository
{
    public Task Add(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default);
    
    public Task<Result<VolunteerRequest, Error>> GetById(
        Guid requestId,
        CancellationToken cancellationToken = default);
}