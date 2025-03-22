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
using P2Project.Discussions.Domain.Entities;
using P2Project.SharedKernel.Errors;
using P2Project.SharedKernel.ValueObjects;

namespace P2Project.Discussions.Application.DiscussionsManagement.Commands.Reopen;

public class ReopenHandler :
    ICommandHandler<Guid, ReopenCommand>
{
    private readonly IValidator<ReopenCommand> _validator;
    private readonly IDiscussionsRepository _discussionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReopenHandler> _logger;

    public ReopenHandler(
        IValidator<ReopenCommand> validator,
        IDiscussionsRepository discussionRepository,
        [FromKeyedServices(Modules.Discussions)] IUnitOfWork unitOfWork,
        ILogger<ReopenHandler> logger)
    {
        _validator = validator;
        _discussionRepository = discussionRepository;
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
        
        var newMessage = Message.Create(
            discussionExist.Value.Id,
            command.UserId,
            Content.Create(command.Comment).Value);
        
        var addMessageResult = discussionExist.Value.AddMessage(newMessage);
        if(addMessageResult.IsFailure)
            return Errors.General.Failure("message").ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Discussion with {Id} reopened by {userId}",
            discussionExist.Value.Id,
            command.UserId);

        return discussionExist.Value.Id;
    }
}