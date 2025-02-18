using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllByAdminId;

namespace P2Project.VolunteerRequests.Web.Requests;

public record GetAllByAdminIdRequest(
    string? Status,
    int Page,
    int PageSize)
{
    public GetAllByAdminIdQuery ToQuery(Guid adminId) =>
        new(adminId, Status, Page, PageSize);
}