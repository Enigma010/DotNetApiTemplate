using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    /// <summary>
    /// Base entity that all entities derive from, unifies the definition that all
    /// entities will have an ID that is the primary identifier
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class Entity<IdType> : IEntity<IdType> where IdType : new() 
    {
        /// <summary>
        /// Creates a new entitty
        /// </summary>
        public Entity()
        {
            Id = new IdType();
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public IdType Id { get; protected set; }
    }
}
