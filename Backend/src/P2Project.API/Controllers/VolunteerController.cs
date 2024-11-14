﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using P2Project.API.Extensions;
using P2Project.Application.Volunteers.CreateVolunteer;

namespace P2Project.API.Controllers
{
    public class VolunteerController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken)
        {
            throw new ApplicationException("Volunteer can't be created");

            var command = new CreateCommand(
                    request.FullName,
                    request.Age,
                    request.Gender,
                    request.Email,
                    request?.Description,
                    request.PhoneNumbers,
                    request?.SocialNetworks,
                    request?.AssistanceDetails);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}