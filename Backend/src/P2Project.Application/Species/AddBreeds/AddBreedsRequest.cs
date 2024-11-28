using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Species.AddBreeds
{
    public record AddBreedsDto(
    IEnumerable<BreedDto> Breeds);

    public record AddBreedsRequest(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto);
}
