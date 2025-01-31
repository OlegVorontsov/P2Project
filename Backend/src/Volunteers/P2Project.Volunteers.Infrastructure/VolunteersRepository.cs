using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;
using P2Project.Volunteers.Infrastructure.DbContexts;

namespace P2Project.Volunteers.Infrastructure
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly VolunteersWriteDbContext _dbContext;
        public VolunteersRepository(VolunteersWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Add(
                                Volunteer volunteer,
                                CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return volunteer.Id;
        }

        public Guid Save(Volunteer volunteer)
        {
            _dbContext.Volunteers.Attach(volunteer);
            return volunteer.Id.Value;
        }

        public Result<Guid, Error> Delete(Volunteer volunteer)
        {
            try
            {
                _dbContext.Volunteers.Remove(volunteer);
            }
            catch (Exception e)
            {
                return Errors.General.Failure();
            }
            return volunteer.Id.Value;
        }

        public async Task<Result<Volunteer, Error>> GetById(
                                 VolunteerId volunteerId,
                                 CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                                            .Include(v => v.Pets)
                                            .FirstOrDefaultAsync(v =>
                                            v.Id == volunteerId,
                                            cancellationToken);
            if (volunteer is null)
                return Errors.General.NotFound(volunteerId);
            return volunteer;
        }
    }
}
