using System;
using System.Collections.Generic;
using CleanArchitecture.Application.ViewModels.Sorting;

namespace CleanArchitecture.Api.Swagger;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class SortableFieldsAttribute<TSortingProvider, TViewModel, TEntity>
    : SwaggerSortableFieldsAttribute
    where TSortingProvider : ISortingExpressionProvider<TViewModel, TEntity>, new()
{
    public override IEnumerable<string> GetFields()
    {
        return new TSortingProvider().GetSortingExpressions().Keys;
    }
}