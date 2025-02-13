using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Domain;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.Create;

public class CreateVolunteerRequestHandler :
    ICommandHandler<Guid, CreateVolunteerRequestCommand>
{
    private readonly IValidator<CreateVolunteerRequestCommand> _validator;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVolunteerRequestHandler> _logger;

    public CreateVolunteerRequestHandler(
        IValidator<CreateVolunteerRequestCommand> validator,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<CreateVolunteerRequestHandler> logger)
    {
        _validator = validator;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var requestId = command.UserId;
        
        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.SecondName,
            command.FullName.LastName).Value;
        
        var volunteerInfo = VolunteerInfo.Create(
            command.VolunteerInfo.Age,
            command.VolunteerInfo.Grade).Value;
        
        var newVolunteerRequest = VolunteerRequest.Create(
            requestId, fullName, volunteerInfo).Value;

        await _volunteerRequestsRepository.Add(newVolunteerRequest, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Volunteer request was created with id {requestId}", requestId);

        return newVolunteerRequest.RequestId;
    }
}