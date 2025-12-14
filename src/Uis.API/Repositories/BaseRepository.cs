using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }

    // ==================== READ OPERATIONS ====================

    public virtual async Task<List<T?>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    // ==================== CREATE OPERATIONS ====================

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }
    public virtual async Task<List<T>> AddRangeAsync(List<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        return entities;
    }

    // ==================== UPDATE OPERATIONS ====================

    public virtual async Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return entity;
    }

    public virtual async Task<List<T>> UpdateRangeAsync(List<T> entities)
    {
        DbSet.UpdateRange(entities);
        return entities;
    }

    // ==================== DELETE OPERATIONS ====================

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    public virtual async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
    }

    public virtual async Task DeleteRangeAsync(List<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

}