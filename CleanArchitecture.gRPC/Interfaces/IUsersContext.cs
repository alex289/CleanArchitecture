using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Users;

namespace CleanArchitecture.gRPC.Interfaces;

public interface IUsersContext
{
    Task<IEnumerable<UserViewModel>> GetUsersByIds(IEnumerable<Guid> ids);
}