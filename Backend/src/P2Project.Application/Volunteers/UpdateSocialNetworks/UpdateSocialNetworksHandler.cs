﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;
        public UpdateSocialNetworksHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }

        public async Task<Result<Guid, Error>> Handle(
            UpdateSocialNetworksCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            var newSocialNetworks = new List<SocialNetwork>();

            var existingSocialNetworks = volunteerResult.Value.SocialNetworks;
            if (existingSocialNetworks != null)
            {
                var oldNetworksToAdd = existingSocialNetworks?
                                    .SocialNetworks
                                    .Select(s => SocialNetwork
                                        .Create(
                                            s.Name,
                                            s.Link).Value);
                if (oldNetworksToAdd != null)
                    newSocialNetworks.AddRange(oldNetworksToAdd);
            }

            if (command.SocialNetworksDto != null)
            {
                var networksToAdd = command
                                    .SocialNetworksDto
                                    .SocialNetworks
                                    .Select(s => SocialNetwork
                                        .Create(
                                            s.Name,
                                            s.Link).Value);
                newSocialNetworks.AddRange(networksToAdd);
            }

            var volunteerNetworks = new VolunteerSocialNetworks(newSocialNetworks);

            volunteerResult.Value.UpdateSocialNetworks(volunteerNetworks);

            var id = await _volunteersRepository.Save(
                            volunteerResult.Value,
                            cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated social networks",
                    id);

            return id;
        }
    }
}