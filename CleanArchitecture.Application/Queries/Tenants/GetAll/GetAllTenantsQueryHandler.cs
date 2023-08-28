using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Queries.Tenants.GetAll;

public sealed class GetAllTenantsQueryHandler :
    IRequestHandler<GetAllTenantsQuery, IEnumerable<TenantViewModel>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetAllTenantsQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<IEnumerable<TenantViewModel>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        return await _tenantRepository
            .GetAllNoTracking()
            .Where(x => !x.Deleted)
            .Select(x => TenantViewModel.FromTenant(x))
            .ToListAsync(cancellationToken);
    }
}