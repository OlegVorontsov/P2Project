using Microsoft.AspNetCore.Mvc;
using P2Project.Domain.Models;
using P2Project.Domain.ValueObjects;

namespace P2Project.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var petPhoto = PetPhoto.Create("path", false).Value;
            List<PetPhoto> petPhotos = [pet, pet, pet];
            return Ok(petPhotos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdatePetPhotoDto dto)
        {
            var request = new UpdatePetPhotoCommand(id, dto);
            return Ok(request);
        }
    }

    public record UpdatePetPhotoCommand(Guid id, UpdatePetPhotoDto updatePetPhotoDto);
    public record UpdatePetPhotoDto(string path, bool isMain);
}
