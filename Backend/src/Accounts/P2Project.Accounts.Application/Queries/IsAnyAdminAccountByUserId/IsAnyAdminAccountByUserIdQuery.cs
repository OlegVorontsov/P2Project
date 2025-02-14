using P2Project.Core.Interfaces.Queries;

namespace P2Project.Accounts.Application.Queries.IsAnyAdminAccountByUserId;

public record IsAnyAdminAccountByUserIdQuery(Guid UserId) : IQuery;