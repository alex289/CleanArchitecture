using CleanArchitecture.gRPC.Interfaces;

namespace CleanArchitecture.gRPC;

public sealed class CleanArchitecture : ICleanArchitecture
{
    private readonly IUsersContext _users;
    private readonly ITenantsContext _tenants;

    public IUsersContext Users => _users;
    public ITenantsContext Tenants => _tenants;

    public CleanArchitecture(
        IUsersContext users,
        ITenantsContext tenants)
    {
        _users = users;
        _tenants = tenants;
        
    }
}
