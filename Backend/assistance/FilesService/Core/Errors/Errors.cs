
namespace FilesService.Core.Errors
{
    public static class Errors
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
            
        public static Error AlreadyExists(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("record.already.exist", $"record with name: {label} already exist");
        }

        public static Error Failure(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Failure(label, $"{label} is invalid");
        }
            
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("lenght.is.invalid",
                $"invalid {label} lenght");
        }
            
        public static Error DeleteConflict(
            Guid? id = null, string? entityTypeName = null)
        {
            var forId = id is null ? " " : $"with id {id} ";
            var type = entityTypeName is null ? "" : $"of type {entityTypeName}";

            return Error.Conflict("Conflict.Constraint", $"Can't delete entity {forId}{type}");
        }
    }
}
