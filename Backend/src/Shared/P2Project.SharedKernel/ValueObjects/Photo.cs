using CSharpFunctionalExtensions;
using P2Project.SharedKernel.Errors;

namespace P2Project.SharedKernel.ValueObjects
{
    public record Photo
    {
        private Photo(string filePath, bool isMain)
        {
            FilePath = filePath;
            IsMain = isMain;
        }

        public string FilePath { get; }
        public bool IsMain { get; }

        public static Result<Photo, Error> Create(
            string filePath, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Errors.Errors.General.ValueIsRequired("FilePath");

            if (filePath.Length > Constants.MAX_BIG_TEXT_LENGTH)
                return Errors.Errors.General.ValueIsRequired("FilePath");

            return new Photo(filePath, isMain);
        }
    }
}
