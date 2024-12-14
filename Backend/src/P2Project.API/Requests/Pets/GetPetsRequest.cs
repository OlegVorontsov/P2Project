using P2Project.Application.Volunteers.Queries.GetPets;

namespace P2Project.API.Requests.Pets
{
    public record GetPetsRequest(
        int Page,
        int PageSize)
    {
        public GetPetsQuery ToQuery() =>
            new (Page, PageSize);
    }
}
