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
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public virtual IQueryable<TEntity> GetAllNoTracking()
    {
        return _dbSet.AsNoTracking();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
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

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _dbSet.AnyAsync(entity => entity.Id == id);
    }
    
    public void Remove(TEntity entity, bool hardDelete = false)
    {
        if (hardDelete)
        {
            _dbSet.Remove(entity);
        }
        else
        {
            entity.Delete();
            _dbSet.Update(entity);
        }
    }

}