using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P2Project.Core.Dtos.Discussions;
using P2Project.Core.Extensions;
using P2Project.Core.Interfaces.Queries;
using P2Project.SharedKernel.Errors;

namespace P2Project.Discussions.Application.DiscussionsManagement.Queries.GetMessageById;

public class GetMessageByIdHandler :
    IQueryValidationHandler<MessageDto, GetMessageByIdQuery>
{
    private readonly IValidator<GetMessageByIdQuery> _validator;
    private readonly IDiscussionsReadDbContext _readDbContext;
    private readonly ILogger<GetMessageByIdHandler> _logger;

    public GetMessageByIdHandler(
        IValidator<GetMessageByIdQuery> validator,
        IDiscussionsReadDbContext readDbContext,
        ILogger<GetMessageByIdHandler> logger)
    {
        _validator = validator;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<MessageDto, ErrorList>> Handle(
        GetMessageByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var messageDto = await _readDbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == query.MessageId,
                cancellationToken);

        if (messageDto is null)
        {
            _logger.LogWarning(
                "Failed to query discussion {discussionId}", query.MessageId);
            return Errors.General.NotFound(query.MessageId).ToErrorList();
        }

        _logger.LogInformation("Discussion {id} queried", query.MessageId);

        return messageDto;
    }
}