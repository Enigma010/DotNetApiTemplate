using App.Entities;
using App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    /// <summary>
    /// The base service interface, the service interface deals with methods to interact with
    /// repository and other services and returns you entities to work with to define your
    /// application logic
    /// </summary>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type that identifies the entity</typeparam>
    public interface IBaseService<EntityType, IdType>
    {
        Task<EntityType> ChangeAsync(IdType id, Func<EntityType, EntityType> changeFunc);
    }
    /// <summary>
    /// The base service object
    /// </summary>
    /// <typeparam name="RepositoryType">The repository type</typeparam>
    /// <typeparam name="EntityType">The entity type</typeparam>
    /// <typeparam name="IdType">The ID type</typeparam>
    public class BaseService<RepositoryType, EntityType, IdType> 
        : IBaseService<EntityType, IdType> 
        where RepositoryType : IBaseRepository<EntityType, IdType>
    {
        /// <summary>
        /// The repository
        /// </summary>
        protected readonly RepositoryType _repository;
        /// <summary>
        /// Crates a new base service
        /// </summary>
        /// <param name="repository">The repository</param>
        public BaseService(RepositoryType repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Handles standard pattern of change/update, standard pattern is load the eneity from
        /// the repository, call a method that updates the entity in process, then call the
        /// repository method to update it in the repository
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <param name="changeFunc">The function to change the entity</param>
        /// <returns>The changed entity</returns>
        public async Task<EntityType> ChangeAsync(IdType id, Func<EntityType, EntityType> changeFunc)
        {
            EntityType entity = await _repository.GetAsync(id);
            changeFunc(entity);
            entity = await _repository.UpdateAsync(entity);
            return entity;
        }
    }
}
