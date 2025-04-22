using System.Reflection;

namespace P2Project.Core.Outbox;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}