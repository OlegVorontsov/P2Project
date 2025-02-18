using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByUserId;

namespace P2Project.VolunteerRequests.Web.Requests;

public record GetAllByUserIdRequest(
    string? Status,
    int Page,
    int PageSize)
{
    public GetAllByUserIdQuery ToQuery(Guid userId) =>
        new(userId, Status, Page, PageSize);
}