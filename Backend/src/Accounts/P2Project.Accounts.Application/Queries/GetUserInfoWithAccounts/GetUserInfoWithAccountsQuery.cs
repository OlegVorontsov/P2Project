using P2Project.Core.Interfaces.Queries;

namespace P2Project.Accounts.Application.Queries.GetUserInfoWithAccounts;

public record GetUserInfoWithAccountsQuery(Guid Id) : IQuery;