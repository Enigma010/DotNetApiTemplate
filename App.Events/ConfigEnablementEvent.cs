namespace App.Events
{
    /// <summary>
    /// Config enablement event
    /// </summary>
    public class ConfigEnablementEvent
    {
        /// <summary>
        /// Config enablement change
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <param name="oldEnabled">The old enabled</param>
        /// <param name="newEnabled">The new enabled</param>
        public ConfigEnablementEvent(
            Guid id,
            bool oldEnabled,
            bool newEnabled)
        {
            Id = id;
            OldEnabled = oldEnabled;
            NewEnabled = newEnabled;
        }
        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// The old enabled value
        /// </summary>
        public bool OldEnabled { get; set; } = false;
        /// <summary>
        /// The new enabled value
        /// </summary>
        public bool NewEnabled { get; set; } = false;
    }
}
