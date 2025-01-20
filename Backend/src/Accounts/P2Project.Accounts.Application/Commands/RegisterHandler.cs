using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain.User;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands;

public class RegisterHandler :
    ICommandHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterHandler> _logger;
    
    public RegisterHandler(
        UserManager<User> userManager,
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        /*var userExists = await _userManager.FindByEmailAsync(command.Email);
        if (userExists != null)
            return Errors.AccountError.AlreadyExist(command.Email).ToErrorList();*/

        var user = new User()
        {
            Email = command.Email,
            UserName = command.Email
        };
            
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User: {userName} created a new account", command.UserName);
            return Result.Success<ErrorList>();
        }
        
        var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        return new ErrorList(errors);
    }
}