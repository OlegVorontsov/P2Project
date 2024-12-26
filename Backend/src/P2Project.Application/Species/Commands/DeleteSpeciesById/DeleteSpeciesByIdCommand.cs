using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Species.Commands.DeleteSpeciesById;

public record DeleteSpeciesByIdCommand(Guid Id) : ICommand;