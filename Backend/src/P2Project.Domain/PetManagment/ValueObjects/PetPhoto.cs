using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.Errors;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record PetPhoto
    {
        private PetPhoto(string filePath, bool isMain)
        {
            FilePath = filePath;
            IsMain = isMain;
        }

        public string FilePath { get; }
        public bool IsMain { get; }

        public static Result<PetPhoto, Error> Create(
            string filePath, bool isMain = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Errors.General.ValueIsRequired("FilePath");

            if (filePath.Length > Constants.MAX_BIG_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("FilePath");

            return new PetPhoto(filePath, isMain);
        }
    }
}
