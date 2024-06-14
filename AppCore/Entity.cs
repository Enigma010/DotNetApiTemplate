using System.Diagnostics.CodeAnalysis;

namespace AppCore
{
    /// <summary>
    /// Base entity that all entities derive from, unifies the definition that all
    /// entities will have an ID that is the primary identifier
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class Entity<IdType> : IEntity<IdType>
    {
        private List<object> _stateChanges = new List<object>();
        /// <summary>
        /// Creates a new entitty
        /// </summary>
        public Entity(Func<IdType> getNewId)
        {
            Id = getNewId();
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        public IdType Id { get; protected set; }
        /// <summary>
        /// Sets the entity state to deleted
        /// </summary>
        public virtual void Deleted()
        {
            throw new InvalidOperationException("Deleted method must be overridden");
        }
        /// <summary>
        /// Adds a state change to the list of state changes that have happened to the entity
        /// </summary>
        /// <param name="stateChanged"></param>
        public void AddStateChange(object stateChanged)
        {
            _stateChanges.Add(stateChanged);
        }
        /// <summary>
        /// Gets all of the state changes
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<object> GetStateChanges()
        {
            return _stateChanges.AsReadOnly();
        }
        /// <summary>
        /// Removes all state changes
        /// </summary>
        public void ClearStateChanges()
        {
            _stateChanges.Clear();
        }
    }
}
