using P2Project.Core.Interfaces.Queries;

namespace P2Project.Species.Application.Queries.GetAllBreedsPaginatedBySpeciesId;

public record GetAllBreedsPaginatedBySpeciesIdQuery(
    Guid SpeciesId,
    string? Name,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;