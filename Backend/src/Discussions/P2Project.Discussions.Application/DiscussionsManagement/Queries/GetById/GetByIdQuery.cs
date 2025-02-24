using P2Project.Core.Interfaces.Queries;

namespace P2Project.Discussions.Application.DiscussionsManagement.Queries.GetById;

public record GetByIdQuery(Guid DiscussionId) : IQuery;