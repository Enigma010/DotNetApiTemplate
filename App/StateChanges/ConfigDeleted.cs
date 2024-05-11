using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.StateChanges
{
    /// <summary>
    /// Config deleted
    /// </summary>
    public class ConfigDeleted : StateChanged<Guid>
    {
        /// <summary>
        /// Config deleted
        /// </summary>
        /// <param name="entity">The config deleted</param>
        public ConfigDeleted(Entity<Guid> entity) : base(entity)
        {
        }
    }
}
