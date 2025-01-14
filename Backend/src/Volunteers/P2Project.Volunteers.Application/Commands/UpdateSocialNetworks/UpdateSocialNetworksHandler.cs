using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.IDs;
using P2Project.Volunteers.Domain.ValueObjects.Volunteers;

namespace P2Project.Volunteers.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksHandler :
        ICommandHandler<Guid, UpdateSocialNetworksCommand>
    {
        private readonly IValidator<UpdateSocialNetworksCommand> _validator;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateSocialNetworksHandler> _logger;
        public UpdateSocialNetworksHandler(
            IValidator<UpdateSocialNetworksCommand> validator,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateSocialNetworksHandler> logger)
        {
            _validator = validator;
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateSocialNetworksCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(
                                      command,
                                      cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerId = VolunteerId.Create(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                var error = Errors.General.NotFound(command.VolunteerId);
                return error.ToErrorList();
            }

            var newSocialNetworks = new List<SocialNetwork>();

            var existingSocialNetworks = volunteerResult.Value.SocialNetworks;
            if (existingSocialNetworks != null)
            {
                var oldNetworksToAdd = existingSocialNetworks?
                                    .Select(s => SocialNetwork
                                        .Create(
                                            s.Name,
                                            s.Link).Value);
                if (oldNetworksToAdd != null)
                    newSocialNetworks.AddRange(oldNetworksToAdd);
            }

            if (command.SocialNetworks != null)
            {
                var networksToAdd = command
                                    .SocialNetworks
                                    .Select(s => SocialNetwork
                                        .Create(
                                            s.Name,
                                            s.Link).Value);
                newSocialNetworks.AddRange(networksToAdd);
            }

            var volunteerNetworks = newSocialNetworks;

            volunteerResult.Value.UpdateSocialNetworks(volunteerNetworks);

            var id = _volunteersRepository.Save(
                            volunteerResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated social networks",
                    id);

            return id;
        }
    }
}
