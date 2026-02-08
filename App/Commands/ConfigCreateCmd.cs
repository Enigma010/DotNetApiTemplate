using System.ComponentModel.DataAnnotations;

namespace App.Commands
{
    public class ConfigCreateCmd
    {
        /// <summary>
        /// The name of the config
        /// </summary>
        [Length(1, 50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;
    }
}
