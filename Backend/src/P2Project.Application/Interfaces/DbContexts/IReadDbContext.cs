using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Interfaces.DbContexts
{
    public interface IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }
        public IQueryable<PetDto> Pets { get; }
    }
}
