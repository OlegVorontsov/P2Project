namespace P2Project.Core.Interfaces.Queries;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}

public interface IQueryHandler<TResponse>
{
    Task<TResponse> Handle(CancellationToken cancellationToken);
}