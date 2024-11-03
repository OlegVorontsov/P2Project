using P2Project.Domain.Shared;

namespace P2Project.Domain.ValueObjects
{
    public class PetPhoto
    {
        private PetPhoto(string path, bool isMain)
        {
            Path = path;
            IsMain = isMain;
        }
        public string Path { get; set; }
        public bool IsMain { get; set; }
        public static Result<PetPhoto> Create(string path, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "Path can't be empty";

            var newPetPhoto = new PetPhoto(path, isMain);

            return newPetPhoto;
        }
    }
}
