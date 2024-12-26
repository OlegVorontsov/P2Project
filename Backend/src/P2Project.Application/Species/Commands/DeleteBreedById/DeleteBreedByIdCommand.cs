using P2Project.Application.Interfaces.Commands;

namespace P2Project.Application.Species.Commands.DeleteBreedById;

public record DeleteBreedByIdCommand(
    Guid SpeciesId,
    Guid BreedId) : ICommand;