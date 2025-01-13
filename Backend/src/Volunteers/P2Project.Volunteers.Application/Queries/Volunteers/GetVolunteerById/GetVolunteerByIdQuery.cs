using P2Project.Core.Interfaces.Queries;

namespace P2Project.Volunteers.Application.Queries.Volunteers.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid Id) : IQuery;