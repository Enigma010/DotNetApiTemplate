using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.StateChanges
{
    /// <summary>
    /// Config created state
    /// </summary>
    public class ConfigCreated: StateChanged<Guid>
    {
        /// <summary>
        /// Config created state
        /// </summary>
        /// <param name="config">The configuration</param>
        public ConfigCreated(Config config) : base(config) 
        {
            Name = config.Name;
            Enabled = config.Enabled;
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name
        {
            get;
            private set;
        } = string.Empty;
        /// <summary>
        /// Whether the configuration is enabled or not
        /// </summary>
        public bool Enabled
        {
            get;
            private set;
        } = false;
    }
}
