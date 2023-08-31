using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Tenants;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tenants.GetAll;

public sealed record GetAllTenantsQuery(PageQuery Query, string SearchTerm = "") :
    IRequest<PagedResult<TenantViewModel>>;