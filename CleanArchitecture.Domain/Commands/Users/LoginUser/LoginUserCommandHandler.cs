using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace CleanArchitecture.Domain.Commands.Users.LoginUser;

public sealed class LoginUserCommandHandler : CommandHandlerBase,
    IRequestHandler<LoginUserCommand, string>
{
    private const double _expiryDurationMinutes = 60;
    private readonly TokenSettings _tokenSettings;

    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IMediatorHandler bus,
        IUnitOfWork unitOfWork,
        INotificationHandler<DomainNotification> notifications,
        IUserRepository userRepository,
        IOptions<TokenSettings> tokenSettings) : base(bus, unitOfWork, notifications)
    {
        _userRepository = userRepository;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (!await TestValidityAsync(request))
        {
            return "";
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no user with email {request.Email}",
                    ErrorCodes.ObjectNotFound));

            return "";
        }

        var passwordVerified = BC.Verify(request.Password, user.Password);

        if (!passwordVerified)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    "The password is incorrect",
                    DomainErrorCodes.User.PasswordIncorrect));

            return "";
        }

        user.SetActive();
        user.SetLastLoggedinDate(DateTimeOffset.Now);

        if (!await CommitAsync())
        {
            return "";
        }

        return BuildToken(
            user,
            _tokenSettings);
    }

    private static string BuildToken(User user, TokenSettings tokenSettings)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenSettings.Secret));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            tokenSettings.Issuer,
            tokenSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_expiryDurationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}