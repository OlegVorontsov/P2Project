using P2Project.Domain.Models;
using P2Project.Domain.ValueObjects;

namespace P2Project.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(string firstName,
                                         string secondName,
                                         string? lastName,
                                         int age,
                                         int gender,
                                         string Email,
                                         string Description,
                                         string registeredDate,
                                         string phoneNumbers,
                                         string? socialNetworks,
                                         string? assistanceDetails)
    {

    }
}
