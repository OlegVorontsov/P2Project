﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.Volunteers.CreateVolunteer;
using P2Project.Application.Volunteers.UpdateMainInfo;

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

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/main-info")]
        public async Task<ActionResult<Guid>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromServices] UpdateMainInfoHandler handler,
            [FromServices] IValidator<UpdateMainInfoRequest> validator,
            CancellationToken cancellationToken)
        {
            var request = new UpdateMainInfoRequest(id);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToValidationErrorResponse();

            var result = await handler.Handle(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}