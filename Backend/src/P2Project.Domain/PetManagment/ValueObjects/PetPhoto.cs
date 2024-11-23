using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record PetPhoto
    {
        // ef navigation
        private PetPhoto(Guid id) { }
        private bool _isDeleted = false;
        private PetPhoto(
            string path,
            bool isMain)
        {
            Id = Guid.Empty;
            Path = path;
            IsMain = isMain;
        }
        public Guid Id { get; private set; }
        public string Path { get; }
        public bool IsMain { get; }
        public static Result<PetPhoto, Error> Create(
            string path,
            bool isMain)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Errors.General.ValueIsInvalid(nameof(Path));

            var newPetPhoto = new PetPhoto(path, isMain);

            return newPetPhoto;
        }
        public void Deleted()
        {
            if (_isDeleted) return;

            _isDeleted = true;
        }
        public void Restored()
        {
            if (!_isDeleted) return;

            _isDeleted = false;
        }
    }
}
