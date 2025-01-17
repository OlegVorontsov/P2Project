using P2Project.Core.Interfaces.Commands;

namespace P2Project.Species.Application.Commands.DeleteBreedById;

public record DeleteBreedByIdCommand(
    Guid SpeciesId,
    Guid BreedId) : ICommand;