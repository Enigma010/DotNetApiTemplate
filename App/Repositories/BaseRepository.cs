using App.Services;
using Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type of that entity</typeparam>
    public interface IBaseRepository<EntityType, IdType>
    {
        Task<EntityType> GetAsync(IdType id);
        Task<IEnumerable<EntityType>> GetAsync();
        Task<IEnumerable<EntityType>> GetAsync(Expression<Func<EntityType, bool>> expression);
        Task<EntityType> InsertAsync(EntityType entity);
        Task<EntityType> UpdateAsync(EntityType entity);
        Task DeleteAsync(IdType id);
    }
    /// <summary>
    /// Base repository object, the repository object interfaces with a data storage
    /// service, like a database.  This base repository supports standarnd operations like get entity,
    /// get all entities, create entity, update endity, delete entity.  Typically all you need to
    /// do is extend this class and that's all you need to do to support standard CRUD operations
    /// </summary>
    /// <typeparam name="RepositoryType">The type of repository</typeparam>
    /// <typeparam name="EntityType">The type of eneity</typeparam>
    /// <typeparam name="IdType">The type of ID</typeparam>
    public class BaseRepository<RepositoryType, EntityType, IdType> : IBaseRepository<EntityType, IdType> where EntityType : IEntity<IdType> where IdType : IComparable
    {
        protected readonly ILogger<RepositoryType> _logger;
        protected readonly IDbClient _client;
        public BaseRepository(IDbClient client,
            ILogger<RepositoryType> logger)
        {
            _client = client;
            _logger = logger;
        }
        /// <summary>
        /// Insert or createa a new entity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The entity created</returns>
        public virtual async Task<EntityType> InsertAsync(EntityType entity) 
        {
            await _client.InsertAsync<EntityType, IdType>(entity);
            return entity;
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The entity updated</returns>
        public virtual async Task<EntityType> UpdateAsync(EntityType entity) 
        {
            await _client.UpdateAsync<EntityType, IdType>(entity);
            return entity;
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(IdType id)
        {
            await _client.DeleteAsync<EntityType, IdType>(id);
        }

        /// <summary>
        /// Gets an entity identified by the ID
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns>The entity</returns>
        public virtual async Task<EntityType> GetAsync(IdType id)
        {
            return await _client.GetAsync<EntityType, IdType>(id);
        }

        /// <summary>
        /// Gets entities based on an expression
        /// </summary>
        /// <typeparam name="EntityType">The entity type</typeparam>
        /// <param name="expression">The filter expression</param>
        /// <returns>The entities that match the expression</returns>
        public virtual async Task<IEnumerable<EntityType>> GetAsync(Expression<Func<EntityType, bool>> expression)
        {
            return await _client.GetAsync(expression);
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All of the entities</returns>
        public virtual async Task<IEnumerable<EntityType>> GetAsync()
        {
            return await _client.GetAsync<EntityType, IdType>();
        }
    }
}