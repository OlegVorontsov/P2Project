using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using P2Project.Domain.IDs;
using P2Project.Domain.Models;

namespace P2Project.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        //[HttpPost]
        //public IActionResult CreatePet(string nickName, string species)
        //{
        //    var petResult = Pet.Create(PetId.NewPetId, nickName, species);

        //    if(petResult.IsFailure)
        //    {
        //        return BadRequest(petResult.Error);
        //    }

        //    var result = Save(petResult.Value);

        //    if(result.IsFailure)
        //    {
        //        return BadRequest(result.Error);
        //    }

        //    return Ok();
        //}
        //public Result Save(Pet pet)
        //{
        //    if (true)
        //    {
        //        return Result.Success();
        //    }
        //    return Result.Failure("Error save to DB");
        //}
    }
}
