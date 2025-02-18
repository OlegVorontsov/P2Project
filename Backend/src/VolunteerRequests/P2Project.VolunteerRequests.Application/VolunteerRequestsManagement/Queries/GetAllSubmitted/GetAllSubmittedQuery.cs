using P2Project.Core.Interfaces.Queries;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmitted;

public record GetAllSubmittedQuery(
    int Page,
    int PageSize) : IQuery;