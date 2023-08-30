using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(DbContext context)
    {
        _dbContext = context;
        DbSet = _dbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public virtual IQueryable<TEntity> GetAllNoTracking()
    {
        return DbSet.AsNoTracking();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await DbSet.AnyAsync(entity => entity.Id == id);
    }

    public void Remove(TEntity entity, bool hardDelete = false)
    {
        if (hardDelete)
        {
            DbSet.Remove(entity);
        }
        else
        {
            entity.Delete();
            DbSet.Update(entity);
        }
    }

    public void RemoveRange(IEnumerable<TEntity> entities, bool hardDelete = false)
    {
        if (hardDelete)
        {
            DbSet.RemoveRange(entities);
            return;
        }

        foreach (var entity in entities)
        {
            entity.Delete();
        }
    }

    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }
}