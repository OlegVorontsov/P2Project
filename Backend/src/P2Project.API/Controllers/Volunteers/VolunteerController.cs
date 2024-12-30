using Microsoft.AspNetCore.Mvc;
using P2Project.API.Controllers.Volunteers.Requests;
using P2Project.API.Extensions;
using P2Project.API.Processor;
using P2Project.Application.Volunteers.Commands.AddPet;
using P2Project.Application.Volunteers.Commands.AddPetPhotos;
using P2Project.Application.Volunteers.Commands.ChangePetStatus;
using P2Project.Application.Volunteers.Commands.Create;
using P2Project.Application.Volunteers.Commands.Delete;
using P2Project.Application.Volunteers.Commands.DeletePetPhotos;
using P2Project.Application.Volunteers.Commands.HardDeletePet;
using P2Project.Application.Volunteers.Commands.SoftDeletePet;
using P2Project.Application.Volunteers.Commands.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.Commands.UpdateMainInfo;
using P2Project.Application.Volunteers.Commands.UpdatePet;
using P2Project.Application.Volunteers.Commands.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.Commands.UpdateSocialNetworks;
using P2Project.Application.Volunteers.Queries.GetFilteredVolunteersWithPagination;
using P2Project.Application.Volunteers.Queries.GetVolunteerById;

namespace P2Project.API.Controllers.Volunteers
{
    public class VolunteerController : ApplicationController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
            [FromServices] GetFilteredVolunteersWithPaginationHandler handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var response = await handler.Handle(query, cancellationToken);

            return Ok(response.Value);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] GetVolunteerByIdHandler handler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolunteerByIdQuery(id);

            var response = await handler.Handle(query, cancellationToken);

            return Ok(response.Value);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateRequest request,
            [FromServices] CreateHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/main-info")]
        public async Task<ActionResult<Guid>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoRequest request,
            [FromServices] UpdateMainInfoHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/phone-numbers")]
        public async Task<ActionResult<Guid>> UpdatePhoneNumbers(
            [FromRoute] Guid id,
            [FromBody] UpdatePhoneNumbersRequest request,
            [FromServices] UpdatePhoneNumbersHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/social-networks")]
        public async Task<ActionResult<Guid>> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialNetworksRequest request,
            [FromServices] UpdateSocialNetworksHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/assistance-details")]
        public async Task<ActionResult<Guid>> UpdateAssistanceDetails(
            [FromRoute] Guid id,
            [FromBody] UpdateAssistanceDetailsRequest request,
            [FromServices] UpdateAssistanceDetailsHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                new DeleteCommand(id), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet")]
        public async Task<IActionResult> AddPet(
            [FromRoute] Guid id,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(id), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> AddPetPhotos(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] AddPetPhotosHandler photosHandler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var fileDtos = fileProcessor.ToUploadFileDtos(files);

            var result = await photosHandler.Handle(
                new AddPetPhotosCommand(
                    volunteerId, petId, fileDtos),
                cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photos")]
        public async Task<IActionResult> DeletePetPhotos(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] DeletePetPhotosHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                new DeletePetPhotosCommand(volunteerId, petId), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok();
        }
        
        [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
        public async Task<IActionResult> UpdatePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] UpdatePetHandler handler,
            [FromBody] UpdatePetRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(volunteerId, petId), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
        public async Task<IActionResult> ChangePetStatus(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] ChangePetStatusRequest request,
            [FromServices] ChangePetStatusHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(
                request.ToCommand(volunteerId, petId), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/soft")]
        public async Task<IActionResult> SoftDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] SoftDeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                new SoftDeletePetCommand(volunteerId, petId), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
        
        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/hard")]
        public async Task<IActionResult> HardDeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] HardDeletePetHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                new HardDeletePetCommand(volunteerId, petId), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}