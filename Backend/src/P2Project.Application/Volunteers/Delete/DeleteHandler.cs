using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using P2Project.Domain.Shared;
using P2Project.Domain.Shared.IDs;

namespace P2Project.Application.Volunteers.Delete
{
    public record DeleteRequest(Guid VolunteerId);
    public record DeleteCommand(Guid VolunteerId);
    public class DeleteValidator :
        AbstractValidator<DeleteRequest>
    {
        public DeleteValidator()
        {
            RuleFor(d => d.VolunteerId).NotEmpty();
        }
    }
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
            DeleteRequest request,
            CancellationToken cancellationToken = default)
        {
            var volunteerId = VolunteerId.CreateVolunteerId(
                request.VolunteerId);

            var volunteerResult = await _volunteersRepository.GetById(
                volunteerId, cancellationToken);
            if(volunteerResult.IsFailure)
                return Errors.General.NotFound(request.VolunteerId);

            var command = new DeleteCommand(volunteerId);

            var id = await _volunteersRepository.Delete(
                volunteerResult.Value, cancellationToken);

            _logger.LogInformation(
                "Volunteer with ID: {id} was deleted",
                id);

            return id;
        }
    }
}
