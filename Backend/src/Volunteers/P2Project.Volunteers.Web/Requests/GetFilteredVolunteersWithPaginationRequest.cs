using P2Project.Volunteers.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

namespace P2Project.Volunteers.Web.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new(Name, SortBy, SortDirection, Page, PageSize);
}