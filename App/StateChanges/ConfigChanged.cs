using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.StateChanges
{
    /// <summary>
    /// Config changed state
    /// </summary>
    public class ConfigChanged : StateChanged<Guid>
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
            Config config,
            string oldName,
            string newName,
            bool oldEnabled,
            bool newEnabled) : base(config)
        {
            OldName = oldName;
            NewName = newName;
            OldEnabled = oldEnabled;
            NewEnabled = newEnabled;
        }
        /// <summary>
        /// The old name
        /// </summary>
        public string OldName { get; private set; }
        /// <summary>
        /// The new name
        /// </summary>
        public string NewName { get; private set; }
        /// <summary>
        /// The old enabled
        /// </summary>
        public bool OldEnabled { get; private set; }
        /// <summary>
        /// The new enabled
        /// </summary>
        public bool NewEnabled { get; private set; }
    }
}
