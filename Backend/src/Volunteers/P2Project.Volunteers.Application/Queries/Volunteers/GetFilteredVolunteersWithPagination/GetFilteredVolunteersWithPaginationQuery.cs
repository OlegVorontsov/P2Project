using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;