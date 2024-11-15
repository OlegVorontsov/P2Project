using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Delete
{
    public class DeleteHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<DeleteHandler> _logger;
        public DeleteHandler(
            IVolunteersRepository volunteersRepository,
            ILogger<DeleteHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            DeleteCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if(volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            var id = await _volunteersRepository.Delete(
                volunteerResult.Value, cancellationToken);

            _logger.LogInformation(
                "Volunteer with ID: {id} was deleted",
                id);

            return id;
        }
    }
}
