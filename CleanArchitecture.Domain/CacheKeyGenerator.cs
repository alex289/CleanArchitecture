using System;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain;

public static class CacheKeyGenerator
{
    public static string GetEntityCacheKey<TEntity>(TEntity entity) where TEntity : Entity
    {
        return $"{typeof(TEntity)}-{entity.Id}";
    }

    public static string GetEntityCacheKey<TEntity>(Guid id) where TEntity : Entity
    {
        return $"{typeof(TEntity)}-{id}";
    }
}