using P2Project.Application.Interfaces.Commands;
using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Species.AddBreeds
{
    public record AddBreedsCommand(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto) : ICommand;
}
