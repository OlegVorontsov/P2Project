using P2Project.Core.Interfaces.Queries;

namespace P2Project.Discussions.Application.DiscussionsManagement.Queries.GetMessageById;

public record GetMessageByIdQuery(Guid MessageId) : IQuery;