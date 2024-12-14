using P2Project.Application.Extensions;
using P2Project.Application.Shared;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public class GetPetsHandler
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsHandler(
            IReadDbContext readDBContext)
        {
            _readDbContext = readDBContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsQuery query,
            CancellationToken cancellationToken)
        {
            var petsDbSet = _readDbContext
                .Pets.AsQueryable();

            return await petsDbSet.ToPagedList(
                query.Page,
                query.PageSize,
                cancellationToken);
        }
    }
}
