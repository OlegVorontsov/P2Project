using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.GetAllSpeciesFilteredPaginated;

public record GetAllSpeciesFilteredPaginatedQuery(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;