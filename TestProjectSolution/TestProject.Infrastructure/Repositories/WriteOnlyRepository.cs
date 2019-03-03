using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Inflector;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TestProject.Domain.Entities;
using TestProject.Domain.Entities.Base;
using TestProject.Domain.Repositories;
using TestProject.Infrastructure.Database;

namespace TestProject.Infrastructure.Repositories
{
    public sealed class WriteOnlyRepository : IWriteOnlyRepository
    {
        private readonly DefaultContext _context;
        private readonly object _locker = new object();

        public WriteOnlyRepository(DefaultContext context)
        {
            _context = context;
        }

        public void SaveEntity<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            lock (_locker)
            {
                var id = (Guid) entity.Id;

                if (id == Guid.Empty)
                {
                    _context.Set<TEntity>().Add(entity);
                    id = (Guid)entity.Id;
                }
                else
                {
                    _context.Set<TEntity>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }

                _context.SaveChanges();

                Log.Information($"The {typeof(TEntity).Name} was created/updated with id = {id}!");
            }
        }

        public async Task SaveEntityAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            var id = (Guid) entity.Id;

            if (id == Guid.Empty)
            {
                await _context.Set<TEntity>().AddAsync(entity);
                id = (Guid) entity.Id;
            }
            else
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            
            await _context.SaveChangesAsync();

            Log.Information($"The {typeof(TEntity).Name} was created/updated with id = {id}!");
        }

        public async Task SaveEntitiesAsync<TEntity>(List<TEntity> entities) where TEntity : class, IBaseEntity
        {
            if(!entities.Any())
                return;

            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void SaveEntities<TEntity>(List<TEntity> entities) where TEntity : class, IBaseEntity
        {
            if (!entities.Any())
                return;

            lock (_locker)
            {
                _context.AddRange(entities);
                _context.SaveChanges();
            }
        }

        public void Delete<TEntity>(object id) where TEntity : class, IBaseEntity
        {
            TEntity entity = _context.Set<TEntity>().Find(id);

            Delete(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
