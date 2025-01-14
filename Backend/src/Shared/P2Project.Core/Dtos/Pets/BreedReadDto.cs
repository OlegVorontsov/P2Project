namespace P2Project.Core.Dtos.Pets;

public record BreedReadDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid SpeciesId { get; init; }
}