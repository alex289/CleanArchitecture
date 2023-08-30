using CleanArchitecture.gRPC.Interfaces;

namespace CleanArchitecture.gRPC;

public sealed class CleanArchitecture : ICleanArchitecture
{
    public CleanArchitecture(
        IUsersContext users,
        ITenantsContext tenants)
    {
        Users = users;
        Tenants = tenants;
    }

    public IUsersContext Users { get; }

    public ITenantsContext Tenants { get; }
}