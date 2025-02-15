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

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.SetRejectStatus;

public class SetRejectStatusHandler :
    ICommandHandler<Guid, SetRejectStatusCommand>
{
    private readonly IValidator<SetRejectStatusCommand> _validator;
    private readonly IDiscussionsAgreement _discussionsAgreement;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IAccountsAgreements _accountsAgreements;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetRejectStatusHandler> _logger;
    
    public SetRejectStatusHandler(
        IValidator<SetRejectStatusCommand> validator,
        IDiscussionsAgreement discussionsAgreement,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<SetRejectStatusHandler> logger,
        IAccountsAgreements accountsAgreements)
    {
        _validator = validator;
        _discussionsAgreement = discussionsAgreement;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _accountsAgreements = accountsAgreements;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        SetRejectStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _volunteerRequestsRepository.GetById(
            command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound(command.RequestId).ToErrorList();
        
        var rejectionComment = RejectionComment.Create(command.Comment).Value;
        existedRequest.Value.SetRejectStatus(rejectionComment);
        
        var messageId = await _discussionsAgreement.CreateMessage(
            command.AdminId, command.Comment, cancellationToken);
        if(messageId.IsFailure)
            return Errors.General.Failure("message").ToErrorList();
        
        await _accountsAgreements.BanUser(existedRequest.Value.UserId, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was rejected", command.RequestId);

        return existedRequest.Value.RequestId;
    }
}