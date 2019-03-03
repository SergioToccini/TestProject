using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.Domain.Entities;
using TestProject.Domain.Entities.Base;
using TestProject.Domain.Repositories;
using TestProject.Infrastructure.Database;

namespace TestProject.Infrastructure.Repositories
{
    public sealed class ReadOnlyRepository : IReadOnlyRepository
    {
        private readonly DefaultContext _context;

        public ReadOnlyRepository(DefaultContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class, IBaseEntity
        {
            return _context.GetDbSet<TEntity>();
        }

        private IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IBaseEntity
        {

            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                includeProperties = includeProperties.Replace(" ", "");

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IBaseEntity
        {
            return GetQueryable<TEntity>(filter, includeProperties).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IBaseEntity
        {
            return await GetQueryable<TEntity>(filter, includeProperties).ToListAsync();
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IBaseEntity
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(object id) where TEntity : class, IBaseEntity
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "") where TEntity : class, IBaseEntity
        {
            return GetQueryable<TEntity>(filter, includeProperties).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "") where TEntity : class, IBaseEntity
        {
            return await GetQueryable<TEntity>(filter, includeProperties).FirstOrDefaultAsync();
        }

    }
}
