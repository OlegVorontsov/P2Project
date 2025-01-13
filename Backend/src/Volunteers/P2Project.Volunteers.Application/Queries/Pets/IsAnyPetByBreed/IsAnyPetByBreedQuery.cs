using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Pets.IsAnyPetByBreed;

public record IsAnyPetByBreedQuery(Guid BreedId) : IQuery;