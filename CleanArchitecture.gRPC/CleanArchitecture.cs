using CleanArchitecture.gRPC.Interfaces;

namespace CleanArchitecture.gRPC;

public sealed class CleanArchitecture : ICleanArchitecture
{
    private readonly IUsersContext _users;

    public IUsersContext Users => _users;

    public CleanArchitecture(IUsersContext users)
    {
        _users = users;
    }
}
