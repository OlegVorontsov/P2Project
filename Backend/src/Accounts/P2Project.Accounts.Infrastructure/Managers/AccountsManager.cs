using CSharpFunctionalExtensions;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain.Accounts;
using P2Project.Accounts.Infrastructure.DbContexts;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Infrastructure.Managers;

public class AccountsManager(AccountsWriteDbContext writeDbContext) : IAccountsManager
{
    public async Task CreateAdminAccount(
        AdminAccount adminAccount, CancellationToken cancellationToken)
    {
        writeDbContext.AdminAccounts.Add(adminAccount);
        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<UnitResult<Error>> CreateVolunteerAccount(
        VolunteerAccount volunteerAccount, CancellationToken cancellationToken)
    {
        writeDbContext.VolunteerAccounts.Add(volunteerAccount);
        var result = await writeDbContext.SaveChangesAsync(cancellationToken);
        
        return result <= 0 ?
            Errors.General.Failure("could.not.create.volunteer_account") :
            Result.Success<Error>();
    }
    
    public async Task CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken)
    {
        writeDbContext.ParticipantAccounts.Add(participantAccount);
        await writeDbContext.SaveChangesAsync(cancellationToken);
    }
}