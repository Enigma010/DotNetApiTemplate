using AppCore;
using AppCore.StateChanges;

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
