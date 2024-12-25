using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;