using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmitted;

namespace P2Project.VolunteerRequests.Web.Requests;

public record GetAllSubmittedRequest(
    int Page, int PageSize)
{
    public GetAllSubmittedQuery ToQuery() =>
        new(Page, PageSize);
}