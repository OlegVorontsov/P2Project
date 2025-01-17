namespace P2Project.Core.Dtos.Pets;

public record SpeciesDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IEnumerable<BreedReadDto> Breeds { get; init; } = default!;
}