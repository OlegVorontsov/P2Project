namespace P2Project.Volunteers.Web.Requests;

public record AddPetPhotosRequest(
    Guid VolunteerId,
    Guid PetId,
    string BucketName);