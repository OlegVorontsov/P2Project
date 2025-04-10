using System.Reflection;

namespace P2Project.Accounts.Agreements;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}