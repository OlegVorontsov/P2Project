using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos.Pets;

namespace P2Project.Application.Species.Commands.AddBreeds
{
    public record AddBreedsCommand(
        Guid SpeciesId,
        IEnumerable<BreedDto> Breeds) : ICommand;
}
