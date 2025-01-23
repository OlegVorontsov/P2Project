using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Pets.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;