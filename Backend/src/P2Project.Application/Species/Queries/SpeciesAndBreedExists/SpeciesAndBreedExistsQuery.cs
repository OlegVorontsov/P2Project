using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.SpeciesAndBreedExists;

public record SpeciesAndBreedExistsQuery(Guid SpeciesId, Guid BreedId) : IQuery;