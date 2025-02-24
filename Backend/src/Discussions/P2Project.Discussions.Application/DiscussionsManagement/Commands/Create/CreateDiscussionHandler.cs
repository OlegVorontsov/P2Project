using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P2Project.Core;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces;
using P2Project.Core.Interfaces.Commands;
using P2Project.Discussions.Application.Interfaces;
using P2Project.Discussions.Domain;
using P2Project.Discussions.Domain.ValueObjects;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Create;

public class CreateDiscussionHandler :
    ICommandHandler<Discussion, CreateDiscussionCommand>
{
    private readonly IValidator<CreateDiscussionCommand> _validator;
    private readonly IDiscussionsRepository _discussionsRepository;
    private readonly ILogger<CreateDiscussionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscussionHandler(
        IValidator<CreateDiscussionCommand> validator,
        IDiscussionsRepository discussionsRepository,
        ILogger<CreateDiscussionHandler> logger,
        [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _discussionsRepository = discussionsRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Discussion, ErrorList>> Handle(
        CreateDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var discussionExist = await _discussionsRepository.GetByParticipantsId(
            command.ReviewingUserId, command.ApplicantUserId, cancellationToken);
        if (discussionExist.IsSuccess)
            return Errors.General.AlreadyExists("discussion").ToErrorList();
        
        var discussionUsers = DiscussionUsers.Create(
            command.ReviewingUserId, command.ApplicantUserId);
        
        var discussion = Discussion.Open(discussionUsers);
        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        var result = await _discussionsRepository.Add(discussion.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Discussion was open with id {discussionId}", discussion.Value.DiscussionId);
        
        return result;
    }
}