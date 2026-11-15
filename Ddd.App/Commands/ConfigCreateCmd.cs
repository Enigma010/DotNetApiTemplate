using System.ComponentModel.DataAnnotations;

namespace Ddd.App.Commands
{
    public class ConfigCreateCmd
    {
        public const string DefaultName = "Default Configuration";
        /// <summary>
        /// The name of the config
        /// </summary>
        [Length(1, 50, ErrorMessage = "The name must be between 1 and 50 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The name is requred")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets the default representation of a configurations
        /// </summary>
        /// <returns></returns>
        public static ConfigCreateCmd Default()
        {
            return new ConfigCreateCmd()
            {
                Name = DefaultName
            };
        }
    }
}
