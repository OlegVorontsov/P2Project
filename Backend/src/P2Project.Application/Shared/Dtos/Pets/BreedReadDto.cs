namespace P2Project.Application.Shared.Dtos.Pets;

public record BreedReadDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid SpeciesId { get; init; }
}