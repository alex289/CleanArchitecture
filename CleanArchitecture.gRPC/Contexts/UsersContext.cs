using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.gRPC.Interfaces;
using CleanArchitecture.Proto.Users;
using CleanArchitecture.Shared.Users;

namespace CleanArchitecture.gRPC.Contexts;

public sealed class UsersContext : IUsersContext
{
    private readonly UsersApi.UsersApiClient _client;

    public UsersContext(UsersApi.UsersApiClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersByIds(IEnumerable<Guid> ids)
    {
        var request = new GetUsersByIdsRequest();

        request.Ids.AddRange(ids.Select(id => id.ToString()));

        var result = await _client.GetByIdsAsync(request);

        return result.Users.Select(user => new UserViewModel(
            Guid.Parse(user.Id),
            user.Email,
            user.FirstName,
            user.LastName,
            string.IsNullOrWhiteSpace(user.DeletedAt) ? null : DateTimeOffset.Parse(user.DeletedAt)));
    }
}