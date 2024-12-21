namespace P2Project.Application.Shared.Dtos.Pets;

public record AddBreedsDto(
    IEnumerable<BreedDto> Breeds);