namespace P2Project.Framework.Authorization;

public static class PermissionsConfig
{
    public static class Volunteers
    {
        public const string Create = "volunteers.create";
        public const string Read = "volunteers.read";
        public const string Update = "volunteers.update";
        public const string Delete = "volunteers.delete";
    }
    
    public static class Pets
    {
        public const string Read = "pets.read";
    }
    
    public static class Species
    {
        public const string Create = "species.create";
        public const string Read = "species.read";
        public const string Update = "species.update";
        public const string Delete = "species.delete";
    }
    
    public static class Breeds
    {
        public const string Create = "breeds.create";
        public const string Read = "breeds.read";
        public const string Update = "breeds.update";
        public const string Delete = "breeds.delete";
    }
    
    public static class Files
    {
        public const string Upload = "files.upload";
        public const string Read = "files.read";
        public const string Delete = "files.delete";
    }
}