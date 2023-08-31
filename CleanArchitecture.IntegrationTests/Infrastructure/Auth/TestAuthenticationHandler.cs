using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.IntegrationTests.Infrastructure.Auth;

public sealed class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
{
    public TestAuthenticationHandler(
        IOptionsMonitor<TestAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authenticationTicket = new AuthenticationTicket(
            new ClaimsPrincipal(Options.Identity),
            new AuthenticationProperties(),
            "Testing");

        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
}