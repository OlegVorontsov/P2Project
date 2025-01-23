using P2Project.Volunteers.Application.Queries.Pets.GetAllPets;

namespace P2Project.Volunteers.Web.Requests
{
    public record GetPetsRequest(
        Guid? VolunteerId,
        Guid? SpeciesId,
        Guid? BreedId,  
        string? NickName,
        string? Color,
        string? City,
        int? WeightFrom,
        int? WeightTo,
        int? HeightFrom,
        int? HeightTo,
        string? SortBy,
        string? SortOrder,
        int Page,
        int PageSize)
    {
        public GetPetsQuery ToQuery() =>
            new (
                VolunteerId,
                SpeciesId,
                BreedId,
                NickName,
                Color,
                City,
                WeightFrom,
                WeightTo,
                HeightFrom,
                HeightTo,
                SortBy,
                SortOrder,
                Page,
                PageSize);
    }
}
