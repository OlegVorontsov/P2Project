using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.IsAnyPetBySpecies;

public record IsAnyPetBySpeciesQuery(Guid SpeciesId) : IQuery;