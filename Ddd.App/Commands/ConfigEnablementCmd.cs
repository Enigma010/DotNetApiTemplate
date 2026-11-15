using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.Commands
{
    /// <summary>
    /// Gets or sets a value indicating whether the configuration is enabled.
    /// </summary>
    public class ConfigEnablementCmd
    {
        /// <summary>
        /// Whether or not the configuration is enabled
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}
