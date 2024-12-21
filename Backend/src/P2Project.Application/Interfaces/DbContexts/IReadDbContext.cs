using P2Project.Application.Shared.Dtos;
using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Dtos.Volunteers;

namespace P2Project.Application.Interfaces.DbContexts
{
    public interface IReadDbContext
    {
        IQueryable<VolunteerDto> Volunteers { get; }
        IQueryable<PetDto> Pets { get; }
    }
}
