﻿using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByIDAsync(Guid id);
        Task<TEntity> InsertWithSaveChangesAsync(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> UpdateWithSaveChangesAsync(TEntity entity);
        Task SaveChangesAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    }
}