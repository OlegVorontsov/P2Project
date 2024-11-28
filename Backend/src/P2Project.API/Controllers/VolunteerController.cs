using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.API.Processor;
using P2Project.Application.Shared.Dtos;
using P2Project.Application.Volunteers.AddPet;
using P2Project.Application.Volunteers.CreatePet;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Application.Volunteers.Delete;
using P2Project.Application.Volunteers.UpdateAssistanceDetails;
using P2Project.Application.Volunteers.UpdateMainInfo;
using P2Project.Application.Volunteers.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.UpdateSocialNetworks;
using P2Project.Application.Volunteers.UploadFilesToPet;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.SpeciesManagment.Entities;

namespace P2Project.API.Controllers
{
    public class VolunteerController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateHandler handler,
            [FromBody] CreateRequest request,
            [FromServices] IValidator<CreateRequest> validator,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(new CreateCommand(
                request.FullName,
                request.Age,
                request.Gender,
                request.Email,
                request?.Description,
                request.PhoneNumbers,
                request?.SocialNetworks,
                request?.AssistanceDetails), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/main-info")]
        public async Task<ActionResult<Guid>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoDto dto,
            [FromServices] UpdateMainInfoHandler handler,
            [FromServices] IValidator<UpdateMainInfoRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateMainInfoRequest(id, dto);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(new UpdateMainInfoCommand(
                request.VolunteerId,
                request.MainInfoDto.FullName,
                request.MainInfoDto.Age,
                request.MainInfoDto.Gender,
                request?.MainInfoDto.Description), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/phone-numbers")]
        public async Task<ActionResult<Guid>> UpdatePhoneNumbers(
            [FromRoute] Guid id,
            [FromBody] UpdatePhoneNumbersDto dto,
            [FromServices] UpdatePhoneNumbersHandler handler,
            [FromServices] IValidator<UpdatePhoneNumbersRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new UpdatePhoneNumbersRequest(id, dto);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(
                new UpdatePhoneNumbersCommand(
                    request.VolunteerId,
                    request.PhoneNumbersDto), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/social-networks")]
        public async Task<ActionResult<Guid>> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialNetworksDto dto,
            [FromServices] UpdateSocialNetworksHandler handler,
            [FromServices] IValidator<UpdateSocialNetworksRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateSocialNetworksRequest(id, dto);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(
                new UpdateSocialNetworksCommand(
                    request.VolunteerId,
                    request.SocialNetworksDto), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/assistance-details")]
        public async Task<ActionResult<Guid>> UpdateAssistanceDetails(
            [FromRoute] Guid id,
            [FromBody] UpdateAssistanceDetailsDto dto,
            [FromServices] UpdateAssistanceDetailsHandler handler,
            [FromServices] IValidator<UpdateAssistanceDetailsRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateAssistanceDetailsRequest(id, dto);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(
                new UpdateAssistanceDetailsCommand(
                    request.VolunteerId,
                    request.AssistanceDetailsDto), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteHandler handler,
            [FromServices] IValidator<DeleteRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new DeleteRequest(id);

            var validationResult = await validator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(new DeleteCommand(
                request.VolunteerId), cancellationToken);

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