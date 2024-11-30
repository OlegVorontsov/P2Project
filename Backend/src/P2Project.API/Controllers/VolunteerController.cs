using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.API.Processor;
using P2Project.Application.Volunteers.AddPet;
using P2Project.Application.Volunteers.CreatePet;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Application.Volunteers.Delete;
using P2Project.Application.Volunteers.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.UpdateMainInfo;
using P2Project.Application.Volunteers.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.UpdateSocialNetworks;
using P2Project.Application.Volunteers.UploadFilesToPet;

namespace P2Project.API.Controllers
{
    public class VolunteerController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateHandler handler,
            [FromBody] CreateRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command, cancellationToken);

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
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

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
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

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
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

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
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

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
            var command = new DeleteCommand(id);

            var result = await handler.Handle(command, cancellationToken);

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
            var command = request.ToCommand(id);

            var result = await handler.Handle(
                command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> UploadFilesToPet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] UploadFilesToPetHandler handler,
            CancellationToken cancellationToken)
        {
            await using var fileProcessor = new FormFileProcessor();
            var fileDtos = fileProcessor.ToUploadFileDtos(files);

            var command = new UploadFilesToPetCommand(
                volunteerId, petId, fileDtos);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}