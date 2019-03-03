using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.Domain.Entities.Base;

namespace TestProject.Domain.Repositories
{
    public interface IReadOnlyRepository
    {
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class, IBaseEntity;

        IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IBaseEntity;
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IBaseEntity;

        TEntity GetById<TEntity>(object id) where TEntity : class, IBaseEntity;
        Task<TEntity> GetByIdAsync<TEntity>(object id) where TEntity : class, IBaseEntity;

        TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "") where TEntity : class, IBaseEntity;
        Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "") where TEntity : class, IBaseEntity;
    }
}
