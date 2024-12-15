using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

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
            var fullPath = path + extension;

            return new FilePath(fullPath);
        }
    }
}
