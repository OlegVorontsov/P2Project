using P2Project.Accounts.Domain.Accounts;

namespace P2Project.Accounts.Application.Interfaces;

public interface IAccountsManager
{
    public Task CreateAdminAccount(
        AdminAccount adminAccount, CancellationToken cancellationToken = default);
    public Task CreateVolunteerAccount(
        VolunteerAccount volunteerAccount, CancellationToken cancellationToken = default);
    public Task CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default);
}