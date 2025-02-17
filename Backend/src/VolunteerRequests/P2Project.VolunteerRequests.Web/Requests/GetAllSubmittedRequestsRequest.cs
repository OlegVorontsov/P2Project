using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Queries.GetAllSubmittedRequests;

namespace P2Project.VolunteerRequests.Web.Requests;

public record GetAllSubmittedRequestsRequest(
    int Page, int PageSize)
{
    public GetAllSubmittedRequestsQuery ToQuery() =>
        new(Page, PageSize);
}