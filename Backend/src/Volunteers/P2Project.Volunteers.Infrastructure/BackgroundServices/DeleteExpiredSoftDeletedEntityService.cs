using Microsoft.EntityFrameworkCore;
using P2Project.SharedKernel;
using P2Project.Volunteers.Application;
using P2Project.Volunteers.Application.Interfaces;
using P2Project.Volunteers.Domain;
using P2Project.Volunteers.Infrastructure.DbContexts;

namespace P2Project.Volunteers.Infrastructure.BackgroundServices;

public class DeleteExpiredSoftDeletedEntityService
{
    private readonly VolunteersWriteDbContext _volunteersWriteDbContext;
    private readonly IVolunteersRepository _volunteersRepository;

    public DeleteExpiredSoftDeletedEntityService(
        VolunteersWriteDbContext volunteersWriteDbContext,
        IVolunteersRepository volunteersRepository)
    {
        _volunteersWriteDbContext = volunteersWriteDbContext;
        _volunteersRepository = volunteersRepository;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var volunteers = await GetVolunteersWithPetsAsync(cancellationToken);

        foreach (var volunteer in volunteers)
        {
            volunteer.DeleteExpiredPets();
            
            if (volunteer.DeletionDateTime != null &&
                DateTime.UtcNow >= volunteer.DeletionDateTime.Value
                    .AddDays(Constants.LIFETIME_AFTER_SOFT_DELETION))
            {
                _volunteersRepository.Delete(volunteer);
            }
        }
        
        await _volunteersWriteDbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task<IEnumerable<Volunteer>> GetVolunteersWithPetsAsync(
        CancellationToken cancellationToken) =>
        await _volunteersWriteDbContext.Volunteers
            .Include(v => v.Pets)
            .ToListAsync(cancellationToken);
}