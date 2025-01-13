using P2Project.Species.Application.Queries.GetAllBreedsPaginatedBySpeciesId;

namespace P2Project.Species.Web.Requests;

public record GetAllBreedsPaginatedBySpeciesIdRequest(
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetAllBreedsPaginatedBySpeciesIdQuery ToQuery(Guid Id)
    {
        return new(Id, Name, SortBy, SortDirection, Page, PageSize);
    }
}