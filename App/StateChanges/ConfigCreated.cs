using App.Entities;
using AppCore.StateChanges;
using System.Runtime.CompilerServices;

namespace App.StateChanges
{
    /// <summary>
    /// Config created state
    /// </summary>
    public class ConfigCreated
    {
        /// <summary>
        /// Config created state
        /// </summary>
        /// <param name="config">The configuration</param>
        public ConfigCreated(Guid id, string name, bool enabled)
        {
            Id = id;
            Name = name;
            Enabled = enabled;
        }
        /// <summary>
        /// The ID of the config
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name
        {
            get;
            set;
        } = string.Empty;
        /// <summary>
        /// Whether the configuration is enabled or not
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        } = false;
    }
}
