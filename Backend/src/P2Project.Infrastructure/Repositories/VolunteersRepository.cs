﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using P2Project.Application.Interfaces.DataBase;
using P2Project.Application.Volunteers;
using P2Project.Domain.PetManagment;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.PetManagment.ValueObjects.Volunteers;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;
using P2Project.Domain.Shared.IDs;
using P2Project.Infrastructure.DbContexts;

namespace P2Project.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly WriteDbContext _dbContext;
        public VolunteersRepository(WriteDbContext dbContext)
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

        public Guid Delete(Volunteer volunteer)
        {
            _dbContext.Volunteers.Remove(volunteer);
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

        public async Task<Result<Volunteer, Error>> GetByFullName(
                                FullName fullName,
                                CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                                            .FirstOrDefaultAsync(v =>
                                            v.FullName == fullName,
                                            cancellationToken);
            if (volunteer is null)
                return Errors.General.NotFound();
            return volunteer;
        }
        public async Task<Result<Volunteer, Error>> GetByEmail(
                                Email email,
                                CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                                            .FirstOrDefaultAsync(v =>
                                            v.Email == email,
                                            cancellationToken);
            if (volunteer is null)
                return Errors.General.NotFound();
            return volunteer;
        }
    }
}
