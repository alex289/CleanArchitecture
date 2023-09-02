using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Queries.Tenants.GetAll;

public sealed class GetAllTenantsQueryHandler :
    IRequestHandler<GetAllTenantsQuery, PagedResult<TenantViewModel>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetAllTenantsQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<PagedResult<TenantViewModel>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantsQuery = _tenantRepository
            .GetAllNoTracking()
            .Include(x => x.Users)
            .Where(x => !x.Deleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            tenantsQuery = tenantsQuery.Where(tenant =>
                tenant.Name.Contains(request.SearchTerm));
        }

        var totalCount = await tenantsQuery.CountAsync(cancellationToken);

        var tenants = await tenantsQuery
            .Skip((request.Query.Page - 1) * request.Query.PageSize)
            .Take(request.Query.PageSize)
            .Select(tenant => TenantViewModel.FromTenant(tenant))
            .ToListAsync(cancellationToken);

        return new PagedResult<TenantViewModel>(
            totalCount, tenants, request.Query.Page, request.Query.PageSize);
    }
}