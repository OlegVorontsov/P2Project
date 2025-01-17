using P2Project.Core.Interfaces.Commands;

namespace P2Project.Species.Application.Commands.DeleteSpeciesById;

public record DeleteSpeciesByIdCommand(Guid Id) : ICommand;