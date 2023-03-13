using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.IntegrationTests.Extensions;
using CleanArchitecture.IntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;
using Xunit.Priority;

namespace CleanArchitecture.IntegrationTests.Controller;

[Collection("IntegrationTests")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public sealed class UserControllerTests : IClassFixture<UserTestFixture>
{
    private readonly UserTestFixture _fixture;

    public UserControllerTests(UserTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact, Priority(0)]
    public async Task Should_Get_No_User()
    {
        var response = await _fixture.ServerClient.GetAsync("user");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<IEnumerable<UserViewModel>>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Should().BeNullOrEmpty();
    }

    [Fact, Priority(5)]
    public async Task Should_Create_User()
    {
        var user = new CreateUserViewModel("test@email.com", "Test", "Email");

        var response = await _fixture.ServerClient.PostAsJsonAsync("user", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();

        message?.Data.Should().NotBeEmpty();

        _fixture.CreatedUserId = message!.Data;
    }

    [Fact, Priority(10)]
    public async Task Should_Get_Created_Users()
    {
        var response = await _fixture.ServerClient.GetAsync("user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Id.Should().Be(_fixture.CreatedUserId);
        content.Email.Should().Be("test@email.com");
        content.Surname.Should().Be("Test");
        content.GivenName.Should().Be("Email");
    }

    [Fact, Priority(15)]
    public async Task Should_Update_User()
    {
        var user = new UpdateUserViewModel(
            _fixture.CreatedUserId,
            "newtest@email.com",
            "NewTest",
            "NewEmail");

        var response = await _fixture.ServerClient.PutAsJsonAsync("user", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UpdateUserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data;

        content.Should().BeEquivalentTo(user);
    }

    [Fact, Priority(20)]
    public async Task Should_Get_Updated_Users()
    {
        var response = await _fixture.ServerClient.GetAsync("user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Id.Should().Be(_fixture.CreatedUserId);
        content.Email.Should().Be("newtest@email.com");
        content.Surname.Should().Be("NewTest");
        content.GivenName.Should().Be("NewEmail");
    }

    [Fact, Priority(25)]
    public async Task Should_Get_One_User()
    {
        var response = await _fixture.ServerClient.GetAsync("user");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<IEnumerable<UserViewModel>>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!.ToList();

        content.Should().ContainSingle();
        content.First().Id.Should().Be(_fixture.CreatedUserId);
        content.First().Email.Should().Be("newtest@email.com");
        content.First().Surname.Should().Be("NewTest");
        content.First().GivenName.Should().Be("NewEmail");
    }

    [Fact, Priority(30)]
    public async Task Should_Delete_User()
    {
        var response = await _fixture.ServerClient.DeleteAsync("user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();

        message?.Data.Should().NotBeEmpty();

        var content = message!.Data;
        content.Should().Be(_fixture.CreatedUserId);
    }

    [Fact, Priority(35)]
    public async Task Should_Get_No_User_Again()
    {
        var response = await _fixture.ServerClient.GetAsync("user");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<IEnumerable<UserViewModel>>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Should().BeNullOrEmpty();
    }
}
