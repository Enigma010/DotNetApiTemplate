using System.ComponentModel.DataAnnotations;

namespace App.Commands
{
    /// <summary>
    /// Represents a set of changes that can be done to a configuration
    /// </summary>
    public class ChangeConfigCmd
    {
        /// <summary>
        /// The name of the configuration
        /// </summary>
        /// 
        [Length(1, 50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Whether or not the configuration is enabled
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}
