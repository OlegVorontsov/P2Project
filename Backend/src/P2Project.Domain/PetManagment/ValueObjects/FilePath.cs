using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record FilePath
    {
        private FilePath(string path)
        {
            Path = path;
        }
        public string Path { get; }
        public static Result<FilePath, Error> Create(
            Guid path, string extension)
        {
            var fullPath = path + "." + extension;

            return new FilePath(fullPath);
        }
        public static Result<FilePath, Error> Create(string filePath)
        {
            var pathParts = filePath.Split('/')
                .Where(p => string.IsNullOrWhiteSpace(p) == false);

            if (pathParts.Count() < 2)
                return Errors.General.ValueIsInvalid("file path");

            if (string.IsNullOrWhiteSpace(filePath))
                return Errors.General.ValueIsInvalid("file path");

            return new FilePath(string.Join("/", pathParts));
        }
    }
}
