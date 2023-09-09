using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanArchitecture.Application.ViewModels.Sorting;

public interface ISortingExpressionProvider<TViewModel, TEntity>
{
    Dictionary<string, Expression<Func<TEntity, object>>> GetSortingExpressions();
}