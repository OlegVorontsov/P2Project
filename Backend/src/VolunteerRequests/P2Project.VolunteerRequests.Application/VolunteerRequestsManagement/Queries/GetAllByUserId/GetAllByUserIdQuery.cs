using P2Project.Core.Interfaces.Queries;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByUserId;

public record GetAllByUserIdQuery(
    Guid UserId,
    string? Status,
    int Page,
    int PageSize) : IQuery;