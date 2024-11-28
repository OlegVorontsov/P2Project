using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using P2Project.Application.Shared;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Delete
{
    public class DeleteHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteHandler> _logger;
        public DeleteHandler(
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteHandler> logger)
        {
            _volunteersRepository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid, Error>> Handle(
            DeleteCommand command,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.Create(
                command.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if(volunteerResult.IsFailure)
                return Errors.General.NotFound(command.VolunteerId);

            volunteerResult.Value.SoftDelete();

            _volunteersRepository.Save(volunteerResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Volunteer with ID: {VolunteerId} was deleted",
                command.VolunteerId);

            return volunteerResult.Value.Id.Value;
        }
    }
}
