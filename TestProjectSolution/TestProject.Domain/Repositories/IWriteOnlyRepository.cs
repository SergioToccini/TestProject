using System.Collections.Generic;
using System.Threading.Tasks;
using TestProject.Domain.Entities.Base;

namespace TestProject.Domain.Repositories
{
    public interface IWriteOnlyRepository
    {
        void SaveEntity<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity;

        Task SaveEntityAsync<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity;

        void SaveEntities<TEntity>(List<TEntity> entities)
            where TEntity : class, IBaseEntity;

        Task SaveEntitiesAsync<TEntity>(List<TEntity> entities)
            where TEntity : class, IBaseEntity;

        void Delete<TEntity>(object id)
            where TEntity : class, IBaseEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity;

        Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity;
    }
}
