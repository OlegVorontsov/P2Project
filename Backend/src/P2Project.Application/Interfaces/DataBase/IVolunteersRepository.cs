using CSharpFunctionalExtensions;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Interfaces.DataBase
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(
                    Volunteer volunteer,
                    CancellationToken cancellationToken = default);
        Guid Save(Volunteer volunteer);
        Guid Delete(Volunteer volunteer);
        Task<Result<Volunteer, Error>> GetById(
                    VolunteerId volunteerId,
                    CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetByFullName(
                    FullName fullName,
                    CancellationToken cancellationToken = default);
        Task<Result<Volunteer, Error>> GetByEmail(
                    Email email,
                    CancellationToken cancellationToken = default);
    }
}