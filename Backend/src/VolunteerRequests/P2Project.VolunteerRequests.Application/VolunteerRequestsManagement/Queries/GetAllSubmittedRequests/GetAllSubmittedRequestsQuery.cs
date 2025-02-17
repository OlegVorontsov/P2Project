using P2Project.Core.Interfaces.Queries;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmittedRequests;

public record GetAllSubmittedRequestsQuery(
    int Page,
    int PageSize) : IQuery;