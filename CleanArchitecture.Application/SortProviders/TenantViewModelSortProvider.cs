using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Application.ViewModels.Sorting;
using CleanArchitecture.Application.ViewModels.Tenants;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.SortProviders;

public sealed class TenantViewModelSortProvider : ISortingExpressionProvider<TenantViewModel, Tenant>
{
    private static readonly Dictionary<string, Expression<Func<Tenant, object>>> s_expressions = new()
    {
        { "id", tenant => tenant.Id },
        { "name", tenant => tenant.Name }
    };

    public Dictionary<string, Expression<Func<Tenant, object>>> GetSortingExpressions()
    {
        return s_expressions;
    }
}