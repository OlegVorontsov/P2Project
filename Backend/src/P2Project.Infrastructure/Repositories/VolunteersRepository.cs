using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.Volunteers;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public VolunteersRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Add(Volunteer volunteer,
                                    CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId)
        {
            var volunteer = await _dbContext.Volunteers
                                            .FirstOrDefaultAsync(v =>
                                            v.Id == volunteerId);
            if (volunteer is null)
                return Errors.General.NotFound(volunteerId);
            return volunteer;
        }
        public async Task<Result<Volunteer, Error>> GetByFullName(FullName fullName)
        {
            var volunteer = await _dbContext.Volunteers
                                            .FirstOrDefaultAsync(v =>
                                            v.FullName == fullName);
            if (volunteer is null)
                return Errors.General.NotFound();
            return volunteer;
        }
        public async Task<Result<Volunteer, Error>> GetByEmail(Email email)
        {
            var volunteer = await _dbContext.Volunteers
                                            .FirstOrDefaultAsync(v =>
                                            v.Email == email);
            if (volunteer is null)
                return Errors.General.NotFound();
            return volunteer;
        }
    }
}
