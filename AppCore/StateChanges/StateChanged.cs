namespace AppCore.StateChanges
{
    /// <summary>
    /// State changed
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    public class StateChanged<IdType> where IdType : new()
    {
        /// <summary>
        /// Creates a state changed
        /// </summary>
        /// <param name="entity">The entity whose state was changed</param>
        public StateChanged(Entity<IdType> entity)
        {
            Id = entity.Id;
        }
        /// <summary>
        /// The ID of the entity
        /// </summary>
        IdType Id { get; set; }
    }
}
