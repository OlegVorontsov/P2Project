using P2Project.Application.Shared.Dtos;

namespace P2Project.Application.Interfaces.DbContexts
{
    public interface IReadDbContext
    {
        IQueryable<VolunteerDto> Volunteers { get; }
        IQueryable<PetDto> Pets { get; }
    }
}
