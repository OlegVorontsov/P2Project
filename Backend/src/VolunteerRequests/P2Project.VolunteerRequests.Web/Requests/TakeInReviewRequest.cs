using P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;

namespace P2Project.VolunteerRequests.Web.Requests;

public record TakeInReviewRequest(Guid RequestId)
{
    public TakeInReviewCommand ToCommand(Guid adminId) =>
        new(adminId, RequestId);
}