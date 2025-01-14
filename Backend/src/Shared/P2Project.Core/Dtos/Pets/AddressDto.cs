
namespace P2Project.Core.Dtos.Pets
{
    public record AddressDto(
        string Region,
        string City,
        string Street,
        string House,
        string? Floor,
        string? Apartment);
}
