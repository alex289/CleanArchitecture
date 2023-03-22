using System;

namespace CleanArchitecture.IntegrationTests.Fixtures;

public sealed class UserTestFixture : TestFixtureBase
{
    public Guid CreatedUserId { get; set; }
    public string CreatedUserEmail { get; set; } = "test@email.com";
    public string CreatedUserPassword { get; set; } = "z8]tnayvd5FNLU9:]AQm";
    public string CreatedUserToken { get; set; } = string.Empty;

    public void EnableAuthentication()
    {
        ServerClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {CreatedUserToken}");
    }
}