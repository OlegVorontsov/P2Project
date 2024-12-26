using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Species.Commands.Create;

namespace P2Project.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(
    NameDto Name,
    IEnumerable<BreedDto> Breeds)
{
    public CreateCommand ToCommand() =>
        new(Name, Breeds);
}
