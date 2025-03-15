using CSharpFunctionalExtensions;
using FilesService.Core.ErrorManagment;

namespace FilesService.Core.ValueObjects
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
