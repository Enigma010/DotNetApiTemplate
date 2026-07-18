using System.ComponentModel.DataAnnotations;

namespace Ddd.App.Commands
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
        [Length(1, 50, ErrorMessage = "The name must be between 1 and 50 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The name is required")]
        public string NewName { get; set; } = string.Empty;
    }
}
