using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Pets.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;