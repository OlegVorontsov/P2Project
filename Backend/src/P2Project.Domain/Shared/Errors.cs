
namespace P2Project.Domain.Shared
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var label = name ?? "value";
                return Error.Validation(label, $"{label} is invalid");
            }
            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $"for id '{id}'";
                return Error.NotFound("record.not.found",
                                      $"record not found {forId}");
            }
            public static Error ValueIsRequired(string? name = null)
            {
                var label = name == null ? "" : " " + name + " ";
                return Error.Validation("lenght.is.invalid",
                                        $"invalid {label} lenght");
            }
        }
        public static class Volunteer
        {
            public static Error AlreadyExist()
            {
                return Error.Validation("record.is.already.exist",
                                        $"Volunteer is already exist");
            }
        }
    }
}
