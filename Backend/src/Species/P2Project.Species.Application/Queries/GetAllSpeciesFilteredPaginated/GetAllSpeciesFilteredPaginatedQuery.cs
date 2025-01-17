using P2Project.Core.Interfaces.Queries;

namespace P2Project.Species.Application.Queries.GetAllSpeciesFilteredPaginated;

public record GetAllSpeciesFilteredPaginatedQuery(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;