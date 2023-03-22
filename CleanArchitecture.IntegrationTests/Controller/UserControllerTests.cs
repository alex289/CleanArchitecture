using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Enums;
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
    public async Task Should_Create_User()
    {
        var user = new CreateUserViewModel(
            _fixture.CreatedUserEmail, 
            "Test", 
            "Email",
            _fixture.CreatedUserPassword);

        var response = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/user", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();

        message?.Data.Should().NotBeEmpty();

        _fixture.CreatedUserId = message!.Data;
    }
    
    [Fact, Priority(5)]
    public async Task Should_Login_User()
    {
        var user = new LoginUserViewModel(
            _fixture.CreatedUserEmail, 
            _fixture.CreatedUserPassword);

        var response = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/user/login", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<string>();

        message?.Data.Should().NotBeEmpty();

        _fixture.CreatedUserToken = message!.Data!;
        _fixture.EnableAuthentication();
    }

    [Fact, Priority(10)]
    public async Task Should_Get_Created_Users()
    {
        var response = await _fixture.ServerClient.GetAsync("/api/v1/user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Id.Should().Be(_fixture.CreatedUserId);
        content.Email.Should().Be("test@email.com");
        content.Surname.Should().Be("Test");
        content.GivenName.Should().Be("Email");
    }
    
    [Fact, Priority(10)]
    public async Task Should_Get_The_Current_Active_Users()
    {
        var response = await _fixture.ServerClient.GetAsync("/api/v1/user/me");

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
            "NewEmail",
            UserRole.User);

        var response = await _fixture.ServerClient.PutAsJsonAsync("/api/v1/user", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UpdateUserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data;

        content.Should().BeEquivalentTo(user);
    }

    [Fact, Priority(20)]
    public async Task Should_Get_Updated_Users()
    {
        var response = await _fixture.ServerClient.GetAsync("/api/v1/user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<UserViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!;

        content.Id.Should().Be(_fixture.CreatedUserId);
        content.Email.Should().Be("newtest@email.com");
        content.Surname.Should().Be("NewTest");
        content.GivenName.Should().Be("NewEmail");
        
        _fixture.CreatedUserEmail = content.Email;
    }
    
    [Fact, Priority(25)]
    public async Task Should_Change_User_Password()
    {
        var user = new ChangePasswordViewModel(
            _fixture.CreatedUserPassword,
            _fixture.CreatedUserPassword + "1");

        var response = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/user/changePassword", user);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<ChangePasswordViewModel>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data;

        content.Should().BeEquivalentTo(user);
        
        // Verify the user can login with the new password
        var login = new LoginUserViewModel(
            _fixture.CreatedUserEmail, 
            _fixture.CreatedUserPassword + "1");

        var loginResponse = await _fixture.ServerClient.PostAsJsonAsync("/api/v1/user/login", login);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginMessage = await loginResponse.Content.ReadAsJsonAsync<string>();

        loginMessage?.Data.Should().NotBeEmpty();
    }

    [Fact, Priority(30)]
    public async Task Should_Get_All_User()
    {
        var response = await _fixture.ServerClient.GetAsync("/api/v1/user");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<IEnumerable<UserViewModel>>();

        message?.Data.Should().NotBeNull();

        var content = message!.Data!.ToList();

        content.Count.Should().Be(2);
        
        var currentUser = content.First(x => x.Id == _fixture.CreatedUserId);
        
        currentUser.Id.Should().Be(_fixture.CreatedUserId);
        currentUser.Role.Should().Be(UserRole.User);
        currentUser.Email.Should().Be("newtest@email.com");
        currentUser.Surname.Should().Be("NewTest");
        currentUser.GivenName.Should().Be("NewEmail");
        
        var adminUser = content.First(x => x.Role == UserRole.Admin);
        
        adminUser.Email.Should().Be("admin@email.com");
        adminUser.Surname.Should().Be("Admin");
        adminUser.GivenName.Should().Be("User");
    }

    [Fact, Priority(35)]
    public async Task Should_Delete_User()
    {
        var response = await _fixture.ServerClient.DeleteAsync("/api/v1/user/" + _fixture.CreatedUserId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsJsonAsync<Guid>();

        message?.Data.Should().NotBeEmpty();

        var content = message!.Data;
        content.Should().Be(_fixture.CreatedUserId);
    }
}
