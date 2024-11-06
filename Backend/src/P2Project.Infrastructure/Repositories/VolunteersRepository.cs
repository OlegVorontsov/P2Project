using Microsoft.EntityFrameworkCore;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;

namespace P2Project.Infrastructure.Repositories
{
    public class VolunteersRepository
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

        public async Task<Result<Volunteer>> GetById(VolunteerId volunteerId,
                                                     CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .ThenInclude(p => p.PetPhotos)
                .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);
            if (volunteer is null)
                return "Volunteer not found";
            return volunteer;   
        }
    }
}
