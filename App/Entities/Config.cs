using App.ActionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    /// <summary>
    /// The configuration object
    /// </summary>
    public class Config : Entity<Guid>
    {
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config() : base()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Whether the configuration is active or not
        /// </summary>
        public bool Enabled { get; private set; } = false;
        /// <summary>
        /// Chagne the configuration
        /// </summary>
        /// <param name="change">The configuration changes</param>
        public void Change(ConfigChangeActionModel change)
        {
            // Support for idempotence, this is done for future support for
            // eventing, i.e. if you event on the name change then if you 
            // call this method with the same name you don't want to trigger
            // a name change event
            if (Name != change.Name)
            {
                Name = change.Name;
            }
            if (Enabled != change.Enabled)
            {
                Enabled = change.Enabled;
            }
        }
    }
}
