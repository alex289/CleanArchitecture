using System.Security.Claims;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Errors;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Notifications;
using CleanArchitecture.Domain.Settings;
using MediatR;
using BC = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Domain.Commands.Users.LoginUser;

public sealed class LoginUserCommandHandler : CommandHandlerBase,
    IRequestHandler<LoginUserCommand, string>
{
    private const double ExpiryDurationMinutes = 30;

    private readonly IUserRepository _userRepository;
    private readonly TokenSettings _tokenSettings;

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

        if (user == null)
        {
            await NotifyAsync(
                new DomainNotification(
                    request.MessageType,
                    $"There is no User with Email {request.Email}",
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
                    DomainErrorCodes.UserPasswordIncorrect));

            return "";
        }

        return BuildToken(
            user.Email,
            user.Role,
            user.Id,
            _tokenSettings);
    }

    public static string BuildToken(string email, UserRole role, Guid id, TokenSettings tokenSettings)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
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
            expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
