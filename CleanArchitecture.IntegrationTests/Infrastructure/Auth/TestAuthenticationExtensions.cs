using System;
using Microsoft.AspNetCore.Authentication;

namespace CleanArchitecture.IntegrationTests.Infrastructure.Auth;

public static class TestAuthenticationExtensions
{
    public static AuthenticationBuilder AddTestAuthentication(
        this AuthenticationBuilder builder,
        Action<TestAuthenticationOptions> configureOptions)
    {
        return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
            "Testing",
            "Test Authentication",
            configureOptions);
    }
}