using System;
using CleanArchitecture.Application.ViewModels.Tenants;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tenants.GetTenantById;

public sealed record GetTenantByIdQuery(Guid TenantId) : IRequest<TenantViewModel?>;