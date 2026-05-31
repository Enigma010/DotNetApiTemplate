using Ddd.App.Db;
using Ddd.App.UnitOfWork;
using DotNetApiLogging;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ddd.App.Core.Repositories
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type of that entity</typeparam>
    public interface IBaseSingletonRepository<EntityType, EntityDtoType, IdType> :
        IGetableSingletonRepository<EntityType, EntityDtoType, IdType>,
        IUpdatableRepository<EntityType, EntityDtoType, IdType>,
        IUnitOfWork

        where EntityDtoType : EntityDto<IdType>
        where EntityType : Entity<EntityDtoType, IdType>
        where IdType : IComparable
    {
        Task<EntityType> InsertAsync(EntityType entity);
        Task DeleteAsync(EntityType entity);
    }
    public class BaseSingletonRepository<RepositoryType, EntityType, EntityDtoType, IdType>
        : IBaseSingletonRepository<EntityType, EntityDtoType, IdType>
        where EntityDtoType : EntityDto<IdType>
        where EntityType : Entity<EntityDtoType, IdType>
        where IdType : IComparable
    {
        protected readonly ILogger<RepositoryType> _logger;
        protected readonly IDbClient _client;

        public bool UseScopedTransactions => false;

        /// <summary>
        /// Creates a new base repository
        /// </summary>
        /// <param name="client">The database client</param>
        /// <param name="logger">The logger</param>
        public BaseSingletonRepository(IDbClient client,
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
            _logger.LogInformationCaller("InsertAsync {@entity}", args: [entity]);
            try
            {
                EntityType existingEntity = await GetAsync();
                throw new DbEntityMultipleSingletonsException<EntityType>(existingEntity);
            }
            catch (DbSingletonEntityNotFoundException)
            {
                _logger.LogInformation("Inserting {Id}", entity.Id);
                await _client.InsertAsync<EntityDtoType, IdType>(entity.GetDto());
                _logger.LogInformation("Inserted {Id}", entity.Id);
                return entity;
            }
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The entity updated</returns>
        public virtual async Task<EntityType> UpdateAsync(EntityType entity)
        {
            _logger.LogInformationCaller("UpdateAsync {@entity}", args: [entity]);
            _logger.LogInformation("Updating {Id}", entity.Id);
            await _client.UpdateAsync<EntityDtoType, IdType>(entity.GetDto());
            _logger.LogInformation("Updated {Id}", entity.Id);
            return entity;
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(EntityType entity)
        {
            _logger.LogInformationCaller("DeleteAsync {@entity}", args: [entity]);
            _logger.LogInformation("Marking entity {Id} as deleted", entity.Id);
            entity.Deleted();
            _logger.LogInformation("Marked entity {Id} as deleted", entity.Id);
            _logger.LogInformation("Deleting {Id}", entity.Id);
            await _client.DeleteAsync<EntityDtoType, IdType>(entity.GetDto());
            _logger.LogInformation("Deleted {Id}", entity.Id);
        }

        /// <summary>
        /// Gets entities based on an expression
        /// </summary>
        /// <typeparam name="EntityType">The entity type</typeparam>
        /// <param name="expression">The filter expression</param>
        /// <returns>The entities that match the expression</returns>
        public virtual async Task<EntityType> GetAsync()
        {
            _logger.LogInformationCaller("GetAsync");
            _logger.LogInformation("Getting all entities");
            IEnumerable<EntityDtoType> entityDtos = await _client.GetAsync<EntityDtoType, IdType>(new Paging());
            _logger.LogInformation("Got all entities");
            IEnumerable<EntityType> entities = entityDtos.Select(GetEntity);
            if (!entities.Any())
            {
                throw new DbSingletonEntityNotFoundException(typeof(EntityType));
            }
            ClearEvents(entities);
            return entities.First();
        }

        /// <summary>
        /// Begins a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Begin()
        {
            await _client.Begin();
        }

        /// <summary>
        /// Commits a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            await _client.Commit();
        }
        /// <summary>
        /// Rollbacks a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            await _client.Rollback();
        }

        /// <summary>
        /// Clears the events on the entities
        /// </summary>
        /// <param name="entities">The entities</param>
        private void ClearEvents(IEnumerable<EntityType> entities)
        {

            _logger.LogInformationCaller("ClearEvents: {@entities}", args: [entities]);
            _logger.LogInformation("Clearing events");
            entities.ToList().ForEach(entity => entity.ClearEvents());
        }

        /// <summary>
        /// Gets an entity object from an entity dto object
        /// </summary>
        /// <param name="entityDto">The entity dto</param>
        /// <returns>The entity</returns>
        /// <exception cref="NullReferenceException">Thrown if unable to get entity from the dto</exception>
        private EntityType GetEntity(EntityDtoType entityDto)
        {
            return (EntityType?)Activator.CreateInstance(typeof(EntityType), entityDto) ?? throw new NullReferenceException();
        }
    }
}
