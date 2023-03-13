using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Proto.Users;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.gRPC;

public sealed class UsersApiImplementation : UsersApi.UsersApiBase
{
    private readonly IUserRepository _userRepository;

    public UsersApiImplementation(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<GetByIdsResult> GetByIds(
        GetByIdsRequest request,
        ServerCallContext context)
    {
        var idsAsGuids = new List<Guid>(request.Ids.Count);

        foreach (var id in request.Ids)
        {
            if (Guid.TryParse(id, out var parsed))
            {
                idsAsGuids.Add(parsed);
            }
        }

        var users = await _userRepository
            .GetAllNoTracking()
            .Where(user => idsAsGuids.Contains(user.Id))
            .Select(user => new GrpcUser
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FirstName = user.GivenName,
                LastName = user.Surname,
                IsDeleted = user.Deleted
            })
            .ToListAsync();

        var result = new GetByIdsResult();

        result.Users.AddRange(users);

        return result;
    }
}