using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Pets.IsAnyPetBySpecies;

public record IsAnyPetBySpeciesQuery(Guid SpeciesId) : IQuery;