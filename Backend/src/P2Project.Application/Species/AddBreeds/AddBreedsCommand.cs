namespace P2Project.Application.Species.AddBreeds
{
    public record AddBreedsCommand(
        Guid SpeciesId,
        AddBreedsDto AddBreedsDto);
}
