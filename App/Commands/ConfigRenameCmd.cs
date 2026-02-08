using System.ComponentModel.DataAnnotations;

namespace App.Commands
{
    /// <summary>
    /// Represents a set of changes that can be done to a configuration
    /// </summary>
    public class ConfigRenameCmd
    {
        /// <summary>
        /// The name of the configuration
        /// </summary>
        /// 
        [Length(1, 50)]
        [Required(AllowEmptyStrings = false)]
        public string NewName { get; set; } = string.Empty;
    }
}
