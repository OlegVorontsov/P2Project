using CSharpFunctionalExtensions;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Application.Interfaces.Queries;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> Handle(
        TQuery query,
        CancellationToken cancellationToken = default);
}