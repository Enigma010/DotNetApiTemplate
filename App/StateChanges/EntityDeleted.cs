using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.StateChanges
{
    /// <summary>
    /// Entity deleted state change
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class EntityDeleted<IdType> : StateChanged<IdType> where IdType : new()
    {
        /// <summary>
        /// Creates a deleted state change
        /// </summary>
        /// <param name="entity">The entity that was deleted</param>
        public EntityDeleted(Entity<IdType> entity) : base(entity)
        {
        }
    }
}
