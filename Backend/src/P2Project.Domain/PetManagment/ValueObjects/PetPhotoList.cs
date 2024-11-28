
namespace P2Project.Domain.PetManagment.ValueObjects
{
    public record PetPhotoList
    {
        public IReadOnlyList<PetPhoto> PetPhotos { get; }

        //EF core constructor
        private PetPhotoList() { }

        public PetPhotoList(IEnumerable<PetPhoto> petPhotos)
        {
            PetPhotos = petPhotos.ToList();
        }
    }
}
