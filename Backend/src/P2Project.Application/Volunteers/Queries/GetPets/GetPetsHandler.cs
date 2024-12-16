using P2Project.Application.Extensions;
using P2Project.Application.Interfaces.DbContexts;
using P2Project.Application.Interfaces.Queries;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Models;

namespace P2Project.Application.Volunteers.Queries.GetPets
{
    public class GetPetsHandler : IQueryHandler<PagedList<PetDto>, GetPetsQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetsHandler(
            IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetPetsQuery query,
            CancellationToken cancellationToken)
        {
            var petsQuery = _readDbContext.Pets;
            
            if (!string.IsNullOrWhiteSpace(query.NickName))
                petsQuery = petsQuery
                    .Where(p => p.NickName.Contains(query.NickName));
            
            var pagedList = await petsQuery.ToPagedList(
                query.Page,
                query.PageSize,
                cancellationToken);

            return pagedList;
        }
    }
}
