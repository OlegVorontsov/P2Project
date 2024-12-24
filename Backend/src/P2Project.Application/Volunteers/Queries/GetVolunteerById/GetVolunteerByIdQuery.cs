using P2Project.Application.Interfaces.Queries;

namespace P2Project.Application.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid Id) : IQuery;