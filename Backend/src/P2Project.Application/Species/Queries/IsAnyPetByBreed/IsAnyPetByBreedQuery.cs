using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Species.Queries.IsAnyPetByBreed;

public record IsAnyPetByBreedQuery(Guid BreedId) : IQuery;