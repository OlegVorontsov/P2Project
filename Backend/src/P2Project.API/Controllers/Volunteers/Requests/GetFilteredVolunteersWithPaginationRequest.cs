using P2Project.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination;

namespace P2Project.API.Controllers.Volunteers.Requests;

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