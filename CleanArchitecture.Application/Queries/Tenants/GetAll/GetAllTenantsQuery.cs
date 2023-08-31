using System.Collections.Generic;
using CleanArchitecture.Application.ViewModels.Tenants;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tenants.GetAll;

public sealed record GetAllTenantsQuery : IRequest<IEnumerable<TenantViewModel>>;