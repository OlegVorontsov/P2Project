using P2Project.Accounts.Domain.Accounts;

namespace P2Project.Accounts.Application.Interfaces;

public interface IAccountsManager
{
    public Task CreateAdminAccount(AdminAccount adminAccount);
    public Task CreateVolunteerAccount(VolunteerAccount volunteerAccount);
    public Task CreateParticipantAccount(ParticipantAccount participantAccount);
}