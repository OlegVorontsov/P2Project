using System.Reflection;

namespace P2Project.VolunteerRequests.Agreements;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}