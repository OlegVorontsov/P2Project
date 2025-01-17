using P2Project.Core.Dtos.Pets;
using P2Project.Core.Dtos.Volunteers;

namespace P2Project.Volunteers.Application
{
    public interface IVolunteersReadDbContext
    {
        IQueryable<VolunteerDto> Volunteers { get; }
        IQueryable<PetDto> Pets { get; }
    }
}
