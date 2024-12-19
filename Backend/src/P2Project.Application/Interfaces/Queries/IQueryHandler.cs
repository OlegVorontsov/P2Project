namespace P2Project.Application.Interfaces.Queries;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(
        TQuery query,
        CancellationToken cancellationToken);
}