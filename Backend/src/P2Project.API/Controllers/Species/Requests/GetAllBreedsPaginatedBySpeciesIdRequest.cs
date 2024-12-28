using P2Project.Application.Species.Queries.GetAllBreedsPaginatedBySpeciesId;

namespace P2Project.API.Controllers.Species.Requests;

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