
namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record PetPhoto
    {
        // ef navigation
        private PetPhoto(Guid id) { }
        private bool _isDeleted = false;
        public PetPhoto(
            FilePath path,
            bool isMain)
        {
            Id = Guid.Empty;
            Path = path;
            IsMain = isMain;
        }

        public Guid Id { get; private set; }
        public FilePath Path { get; }
        public bool IsMain { get; }

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
