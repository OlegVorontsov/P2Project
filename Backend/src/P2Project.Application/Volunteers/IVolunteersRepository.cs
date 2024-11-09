using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
        Task<Result<Volunteer>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    }
}