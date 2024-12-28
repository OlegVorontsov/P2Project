using P2Project.Application.Shared.Dtos.Pets;
using P2Project.Application.Shared.Dtos.Volunteers;

namespace P2Project.Application.Interfaces.DbContexts.Volunteers
{
    public interface IVolunteersReadDbContext
    {
        IQueryable<VolunteerDto> Volunteers { get; }
        IQueryable<PetDto> Pets { get; }
    }
}
