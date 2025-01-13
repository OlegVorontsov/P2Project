using P2Project.Core.Interfaces.Queries;

namespace P2Project.Species.Application.Queries.SpeciesAndBreedExists;

public record SpeciesAndBreedExistsQuery(
    Guid SpeciesId, Guid BreedId) : IQuery;