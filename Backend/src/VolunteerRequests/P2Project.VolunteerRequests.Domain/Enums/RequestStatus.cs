namespace P2Project.VolunteerRequests.Domain.Enums;

public enum RequestStatus
{
    Submitted,          //создание протестировано
    Rejected,           //отклонение протестировано
    RevisionRequired,   //на доработку протестировано
    Approved,
    OnReview            //создание заявки протестировано
}