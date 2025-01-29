using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using P2Project.Accounts.Application.Interfaces;
using P2Project.Accounts.Domain;
using P2Project.Core.Interfaces.Commands;
using P2Project.SharedKernel.Errors;

namespace P2Project.Accounts.Application.Commands.Login;

public class LoginHandler :
    ICommandHandler<string, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByEmailAsync(command.Email);
        if (userExist == null)
            return Errors.General.NotFound().ToErrorList();
        
        var passwordConfirmed = await _userManager
            .CheckPasswordAsync(userExist, command.Password);
        if(!passwordConfirmed)
            return Errors.AccountError.InvalidCredentials().ToErrorList();
        
        var token = await _tokenProvider.GenerateAccessToken(userExist);

        return token;
    }
}