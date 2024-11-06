using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public record PetPhoto
    {
        private PetPhoto(string path, bool isMain)
        {
            Path = path;
            IsMain = isMain;
        }
        public string Path { get; }
        public bool IsMain { get; }
        public static Result<PetPhoto> Create(string path, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "Path can't be empty";

            var newPetPhoto = new PetPhoto(path, isMain);

            return newPetPhoto;
        }
    }
}
