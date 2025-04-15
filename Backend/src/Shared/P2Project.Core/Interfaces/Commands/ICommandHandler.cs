using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.Core.Interfaces.Commands;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public interface ICommandVoidHandler<in TCommand> where TCommand : ICommand
{
    public Task Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public interface ICommandResponseHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<TResponse> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}