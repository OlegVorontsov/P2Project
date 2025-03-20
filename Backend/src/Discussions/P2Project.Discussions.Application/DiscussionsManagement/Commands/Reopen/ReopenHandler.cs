using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Discussions.Agreements;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Reopen;

public class ReopenHandler :
    ICommandHandler<Guid, ReopenCommand>
{
    private readonly IValidator<ReopenCommand> _validator;
    private readonly IDiscussionsRepository _discussionRepository;
    private readonly IDiscussionsAgreement _discussionsAgreement;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReopenHandler> _logger;

    public ReopenHandler(
        IValidator<ReopenCommand> validator,
        IDiscussionsRepository discussionRepository,
        IDiscussionsAgreement discussionsAgreement,
        [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork,
        ILogger<ReopenHandler> logger)
    {
        _validator = validator;
        _discussionRepository = discussionRepository;
        _discussionsAgreement = discussionsAgreement;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        ReopenCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var discussionExist = await _discussionRepository.GetById(
            command.DiscussionId, cancellationToken);
        if (discussionExist.IsFailure)
            return Errors.General.NotFound(command.DiscussionId).ToErrorList();
        
        if(discussionExist.Value.Status == DiscussionStatus.Open)
            return Errors.General.Failure("already.opened").ToErrorList();
        
        discussionExist.Value.Reopen();
        
        var messageId = await _discussionsAgreement.CreateMessage(
            command.UserId, discussionExist.Value.DiscussionUsers.ReviewingUserId,
            command.Comment,
            cancellationToken);
        if(messageId.IsFailure)
            return Errors.General.Failure("message").ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Discussion with {Id} reopened by {userId}",
            discussionExist.Value.Id,
            command.UserId);

        return discussionExist.Value.Id;
    }
}