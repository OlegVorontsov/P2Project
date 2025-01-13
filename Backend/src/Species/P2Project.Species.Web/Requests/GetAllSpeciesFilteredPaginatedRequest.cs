using P2Project.Species.Application.Queries.GetAllSpeciesFilteredPaginated;

namespace P2Project.Species.Web.Requests;

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