using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Application.Volunteers.Delete;
using P2Project.Application.Volunteers.UpdateMainInfo;
using P2Project.Application.Volunteers.UpdatePhoneNumbers;
using P2Project.Application.Volunteers.UpdateSocialNetworks;

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
            [FromServices] IValidator<UpdateMainInfoRequest> requestValidator,
            [FromServices] IValidator<UpdateMainInfoDto> dtoValidator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateMainInfoRequest(id, dto);

            var requestValidationResult = await requestValidator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (requestValidationResult.IsValid == false)
                return requestValidationResult.ToValidationErrorResponse();

            var dtoValidationResult = await dtoValidator.ValidateAsync(
                                      dto,
                                      cancellationToken);
            if (dtoValidationResult.IsValid == false)
                return dtoValidationResult.ToValidationErrorResponse();

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
            [FromServices] IValidator<UpdatePhoneNumbersRequest> requestValidator,
            [FromServices] IValidator<UpdatePhoneNumbersDto> dtoValidator,
            CancellationToken cancellationToken)
        {
            var request = new UpdatePhoneNumbersRequest(id, dto);

            var requestValidationResult = await requestValidator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (requestValidationResult.IsValid == false)
                return requestValidationResult.ToValidationErrorResponse();

            var dtoValidationResult = await dtoValidator.ValidateAsync(
                                      dto,
                                      cancellationToken);
            if (dtoValidationResult.IsValid == false)
                return dtoValidationResult.ToValidationErrorResponse();

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
            [FromServices] IValidator<UpdateSocialNetworksRequest> requestValidator,
            [FromServices] IValidator<UpdateSocialNetworksDto> dtoValidator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateSocialNetworksRequest(id, dto);

            var requestValidationResult = await requestValidator.ValidateAsync(
                                                  request,
                                                  cancellationToken);
            if (requestValidationResult.IsValid == false)
                return requestValidationResult.ToValidationErrorResponse();

            var dtoValidationResult = await dtoValidator.ValidateAsync(
                                      dto,
                                      cancellationToken);
            if (dtoValidationResult.IsValid == false)
                return dtoValidationResult.ToValidationErrorResponse();

            var result = await handler.Handle(
                new UpdateSocialNetworksCommand(
                    request.VolunteerId,
                    request.SocialNetworksDto), cancellationToken);

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
    }
}