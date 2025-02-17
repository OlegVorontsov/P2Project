using P2Project.Core.Interfaces.Queries;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByAdminId;

public record GetAllByAdminIdQuery(
    Guid AdminId,
    string? Status,
    int Page,
    int PageSize) : IQuery;