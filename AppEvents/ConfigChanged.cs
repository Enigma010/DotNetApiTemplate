using EventBus;

namespace AppEvents
{
    /// <summary>
    /// Config changed state
    /// </summary>
    [Event]
    public class ConfigChanged
    {
        /// <summary>
        /// Config state changed
        /// </summary>
        /// <param name="config">The configuration</param>
        /// <param name="oldName">The old name</param>
        /// <param name="newName">The new name</param>
        /// <param name="oldEnabled">The old enabled</param>
        /// <param name="newEnabled">The new enabled</param>
        public ConfigChanged(
            Guid id,
            string oldName,
            string newName,
            bool oldEnabled,
            bool newEnabled)
        {
            Id = id;
            OldName = oldName;
            NewName = newName;
            OldEnabled = oldEnabled;
            NewEnabled = newEnabled;
        }
        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// The old name
        /// </summary>
        public string OldName { get; set; } = string.Empty;
        /// <summary>
        /// The new name
        /// </summary>
        public string NewName { get; set; } = string.Empty;
        /// <summary>
        /// The old enabled
        /// </summary>
        public bool OldEnabled { get; set; } = false;
        /// <summary>
        /// The new enabled
        /// </summary>
        public bool NewEnabled { get; set; } = false;
    }
}
