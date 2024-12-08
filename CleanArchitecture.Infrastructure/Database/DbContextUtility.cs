using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database;

public partial class ApplicationDbContext
{
    public static class DbContextUtility
    {
        public const string IsDeletedProperty = "DeletedAt";

        public static readonly MethodInfo PropertyMethod = typeof(EF)
            .GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)
            !.MakeGenericMethod(typeof(DateTimeOffset?));

        public static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(PropertyMethod, parm, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(null));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }
    }
}