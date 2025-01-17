using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Interfaces.Queries;

public interface IQueryValidationHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Handle(
        TQuery query,
        CancellationToken cancellationToken = default);
}