using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity

{   // READ
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    // CREATE
    Task<T> AddAsync(T entity);
    Task<List<T>> AddRangeAsync(List<T> entities);

    // UPDATE
    Task<T> UpdateAsync(T entity);
    Task<List<T>> UpdateRangeAsync(List<T> entities);

    // DELETE
    Task DeleteAsync(int id);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(List<T> entities);
}
