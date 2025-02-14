using P2Project.Core.Dtos.Accounts;

namespace P2Project.Accounts.Application.Interfaces;

public interface IAccountsReadDbContext
{
    public IQueryable<UserDto> Users { get; }
    public IQueryable<AdminAccountDto> AdminAccounts { get; }
    /*public IQueryable<VolunteerAccountDto> VolunteerAccounts { get; }
    public IQueryable<ParticipantAccountDto> ParticipantAccounts { get; }*/
}