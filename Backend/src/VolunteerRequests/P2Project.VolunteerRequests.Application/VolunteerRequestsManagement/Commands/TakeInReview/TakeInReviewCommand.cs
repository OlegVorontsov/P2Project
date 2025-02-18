using P2Project.Core.Interfaces.Commands;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;

public record TakeInReviewCommand(Guid AdminId, Guid RequestId) : ICommand;