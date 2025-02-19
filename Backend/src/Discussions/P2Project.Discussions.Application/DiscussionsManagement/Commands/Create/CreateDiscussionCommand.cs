using P2Project.Core.Interfaces.Commands;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Create;

public record CreateDiscussionCommand(
    Guid ReviewingUserId, Guid ApplicantUserId) : ICommand;