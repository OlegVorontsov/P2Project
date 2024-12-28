﻿
namespace P2Project.Domain.Shared.Errors
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
            public static Error DeleteConflict(
                Guid? id = null, string? entityTypeName = null)
            {
                var forId = id is null ? " " : $"with id {id} ";
                var type = entityTypeName is null ? "" : $"of type {entityTypeName}";

                return Error.Conflict("Conflict.Constraint", $"Can't delete entity {forId}{type}");
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
        public static class Species
        {
            public static Error AlreadyExist()
            {
                return Error.Validation("record.is.already.exist",
                                        $"Species is already exist");
            }
            public static Error NonExistantSpecies(Guid id) =>
                Error.NotFound("Species.NonExistantSpecies", $"Non-existant species (id = {id})");
            public static Error NonExistantBreed(Guid id) =>
                Error.NotFound("Species.NonExistantBreed", $"Non-existant breed (id = {id})");
            public static Error BreedDelete(Guid id) =>
                Error.Unexpected("Breed.DeletingFail", $"Failed to delete breed with id {id}");
        }
        
        public static class Breed
        {
            public static Error AlreadyExist()
            {
                return Error.Validation("record.is.already.exist",
                    $"Breed is already exist");
            }
        }
    }
}
