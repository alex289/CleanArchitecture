using CleanArchitecture.gRPC.Interfaces;

namespace CleanArchitecture.gRPC;

public interface ICleanArchitecture
{
    IUsersContext Users { get; }
    ITenantsContext Tenants { get; }
}