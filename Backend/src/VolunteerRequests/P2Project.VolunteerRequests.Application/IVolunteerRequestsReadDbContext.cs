using P2Project.Core.Dtos.VolunteerRequests;

namespace P2Project.VolunteerRequests.Application;

public interface IVolunteerRequestsReadDbContext
{
    IQueryable<VolunteerRequestDto> VolunteerRequests { get; }
}