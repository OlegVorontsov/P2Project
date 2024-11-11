using CSharpFunctionalExtensions;
using P2Project.Domain.Models;
using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record PetPhoto
    {
        // ef navigation
        private PetPhoto(Guid id) { }
        private PetPhoto(Guid id,
                         string path,
                         bool isMain)
        {
            Id = id;
            Path = path;
            IsMain = isMain;
        }
        public Guid Id { get; private set; }
        public string Path { get; }
        public bool IsMain { get; }
        public static Result<PetPhoto, Error> Create(Guid id,
                                                     string path,
                                                     bool isMain)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Errors.General.ValueIsInvalid(nameof(Path));

            var newPetPhoto = new PetPhoto(id, path, isMain);

            return newPetPhoto;
        }
    }
}
