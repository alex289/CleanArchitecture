using System;
using CleanArchitecture.Domain.Notifications;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Infrastructure.Tests;

public sealed class DomainNotificationTests
{
    [Fact]
    public void Should_Create_DomainNotification_Instance()
    {
        var key = "Key";
        var value = "Value";
        var code = "Code";

        var domainNotification = new DomainNotification(
            key, value, code);

        domainNotification.Key.Should().Be(key);
        domainNotification.Value.Should().Be(value);
        domainNotification.Should().NotBe(default(Guid));
        domainNotification.Code.Should().Be(code);
    }

    [Fact]
    public void Should_Create_DomainNotification_Overload_Instance()
    {
        var key = "Key";
        var value = "Value";
        var code = "Code";

        var domainNotification = new DomainNotification(
            key, value, code);

        domainNotification.Key.Should().Be(key);
        domainNotification.Value.Should().Be(value);
        domainNotification.Code.Should().Be(code);
        domainNotification.Should().NotBe(default(Guid));
    }
}