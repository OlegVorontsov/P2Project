using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Agreements;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Discussions.Agreements;
using P2Project.SharedKernel.Errors;
using P2Project.VolunteerRequests.Application.Interfaces;
using P2Project.VolunteerRequests.Domain.ValueObjects;

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRevisionRequiredStatus;

public class SetRevisionRequiredStatusHandler :
    ICommandHandler<Guid, SetRevisionRequiredStatusCommand>
{
    private readonly IValidator<SetRevisionRequiredStatusCommand> _validator;
    private readonly IAdminAccountsAgreement _adminAccountsAgreement;
    private readonly IDiscussionsAgreement _discussionsAgreement;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetRevisionRequiredStatusHandler> _logger;

    public SetRevisionRequiredStatusHandler(
        IValidator<SetRevisionRequiredStatusCommand> validator,
        IAdminAccountsAgreement adminAccountsAgreement,
        IDiscussionsAgreement discussionsAgreement,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<SetRevisionRequiredStatusHandler> logger)
    {
        _validator = validator;
        _adminAccountsAgreement = adminAccountsAgreement;
        _discussionsAgreement = discussionsAgreement;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetRevisionRequiredStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var adminExist = await _adminAccountsAgreement.IsAnyAdminAccountByUserId(
            command.AdminId, cancellationToken);
        if (adminExist == false)
            return Errors.General.NotFound(command.AdminId).ToErrorList();
        
        var existedRequest = await _volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        var rejectionComment = RejectionComment.Create(command.Comment).Value;
        existedRequest.Value.SetRevisionRequiredStatus(rejectionComment);
        
        var messageId = await _discussionsAgreement.CreateMessage(
            command.AdminId, command.Comment, cancellationToken);
        if(messageId.IsFailure)
            return Errors.General.Failure("message").ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was rejected", command.RequestId);

        return existedRequest.Value.RequestId;
    }
}