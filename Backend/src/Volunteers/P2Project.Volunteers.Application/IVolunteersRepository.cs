using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Volunteers.Application
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(
                    Volunteer volunteer,
                    CancellationToken cancellationToken = default);
        
        Guid Save(Volunteer volunteer);
        
        Result<Guid, Error> Delete(Volunteer volunteer);
        
        Task<Result<Volunteer, Error>> GetById(
                    VolunteerId volunteerId,
                    CancellationToken cancellationToken = default);
    }
}