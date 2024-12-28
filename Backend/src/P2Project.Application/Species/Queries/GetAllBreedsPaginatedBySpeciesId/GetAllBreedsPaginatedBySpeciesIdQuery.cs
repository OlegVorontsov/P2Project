using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.GetAllBreedsPaginatedBySpeciesId;

public record GetAllBreedsPaginatedBySpeciesIdQuery(
    Guid SpeciesId,
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;