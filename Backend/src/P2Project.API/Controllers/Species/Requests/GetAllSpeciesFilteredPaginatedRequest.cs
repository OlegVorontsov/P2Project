using P2Project.Application.Species.Queries.GetAllSpeciesFilteredPaginated;

namespace P2Project.API.Controllers.Species.Requests;

public record GetAllSpeciesFilteredPaginatedRequest(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetAllSpeciesFilteredPaginatedQuery ToQuery()
    {
        return new(Name, SortBy, SortDirection, Page, PageSize);
    }
}