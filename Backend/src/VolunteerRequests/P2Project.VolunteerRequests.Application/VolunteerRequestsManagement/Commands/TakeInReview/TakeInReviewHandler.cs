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

namespace P2Project.VolunteerRequests.Application.VolunteerRequestsManagement.Commands.TakeInReview;

public class TakeInReviewHandler :
    ICommandHandler<Guid, TakeInReviewCommand>
{
    private readonly IValidator<TakeInReviewCommand> _validator;
    private readonly IAdminAccountsAgreement _adminAccountsAgreement;
    private readonly IDiscussionsAgreement _discussionsAgreement;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TakeInReviewHandler> _logger;

    public TakeInReviewHandler(
        IValidator<TakeInReviewCommand> validator,
        IAdminAccountsAgreement adminAccountsAgreement,
        IDiscussionsAgreement discussionsAgreement,
        IVolunteerRequestsRepository volunteerRequestsRepository,
        [FromKeyedServices(Modules.VolunteerRequests)] IUnitOfWork unitOfWork,
        ILogger<TakeInReviewHandler> logger)
    {
        _validator = validator;
        _adminAccountsAgreement = adminAccountsAgreement;
        _discussionsAgreement = discussionsAgreement;
        _volunteerRequestsRepository = volunteerRequestsRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        TakeInReviewCommand command,
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
        
        var newDiscussionId = await _discussionsAgreement.CreateDiscussion(
            command.AdminId,
            existedRequest.Value.UserId,
            cancellationToken);
        if(newDiscussionId.IsFailure)
            return Errors.General.Failure("discussion").ToErrorList();
            
        existedRequest.Value.TakeInReview(command.AdminId, newDiscussionId.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request {requestId} was taken in review", command.RequestId);

        return existedRequest.Value.RequestId;
    }
}