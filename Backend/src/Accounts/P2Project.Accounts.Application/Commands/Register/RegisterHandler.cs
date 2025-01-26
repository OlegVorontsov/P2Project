using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Domain;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.Register;

/*public class RegisterHandler :
    ICommandHandler<string, RegisterCommand>
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
    
    public async Task<Result<string, ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            Email = command.Email,
            UserName = command.Email
        };
            
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User: {userName} created a new account", command.UserName);
            
            await _userManager.AddToRoleAsync(user, "participant");
            
            return command.UserName;
        }
        
        var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        return new ErrorList(errors);
    }
}*/