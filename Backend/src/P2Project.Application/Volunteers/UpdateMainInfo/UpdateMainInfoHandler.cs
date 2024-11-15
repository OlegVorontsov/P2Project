using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.PetManagment.ValueObjects;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdateMainInfoHandler> _logger;

        public UpdateMainInfoHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<UpdateMainInfoHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            UpdateMainInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                                          command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                                        volunteerId,
                                        cancellationToken);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            var fullName = FullName.Create(
                                    command.FullName.FirstName,
                                    command.FullName.SecondName,
                                    command.FullName.LastName).Value;

            var description = Description.Create(command.Description).Value;

            volunteerResult.Value.UpdateMainInfo(fullName, description);

            var id = await _volunteersRepository.Save(
                                        volunteerResult.Value,
                                        cancellationToken);

            _logger.LogInformation(
                    "For volunteer with ID: {id} was updated main info to " +
                    "full name: {SecondName} {FirstName} " +
                    "{LastName} " +
                    "description: {Value}",
                    id,
                    fullName.SecondName,
                    fullName.FirstName,
                    fullName.LastName,
                    description.Value);

            return id;
        }
    }
}
