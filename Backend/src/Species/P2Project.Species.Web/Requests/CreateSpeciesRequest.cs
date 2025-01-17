using P2Project.Core.Dtos.Pets;
using P2Project.Species.Application.Commands.Create;

namespace P2Project.Species.Web.Requests;

public record CreateSpeciesRequest(
    NameDto Name,
    IEnumerable<BreedDto> Breeds)
{
    public CreateCommand ToCommand() =>
        new(Name, Breeds);
}
