using Ddd.App.Commands;
using Ddd.App.Core;
using Ddd.App.Events;
using Ddd.App.Repositories.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace Ddd.App.Entities
{
    /// <summary>
    /// The configuration object
    /// </summary>
    public class Config : Entity<ConfigDto, Guid>
    {
        /// <summary>
        /// Creates a new configuration loaded from the repository
        /// </summary>
        /// <param name="dto"></param>
        public Config(ConfigDto dto) : base(dto)
        {
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config() : base(Guid.NewGuid)
        {
            AddEvent(new ConfigCreatedEvent(_dto.Id, _dto.Name, _dto.Enabled));
        }
        /// <summary>
        /// Createa a new configuration
        /// </summary>
        public Config(string name, bool enabled = false) : base(Guid.NewGuid)
        {
            _dto.Name = name;
            _dto.Enabled = enabled;
            AddEvent(new ConfigCreatedEvent(_dto.Id, _dto.Name, _dto.Enabled));
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        [Length(1, 50)]
        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get
            {
                return _dto.Name;
            }
        }

        /// <summary>
        /// Set the config to be deleted
        /// </summary>
        public override void Deleted()
        {
            AddEvent(new ConfigDeletedEvent(_dto.Id));
        }

        /// <summary>
        /// Chagne the configuration
        /// </summary>
        /// <param name="cmd">The configuration changes</param>
        public void Rename(ConfigRenameCmd cmd)
        {
            if (Name != cmd.NewName)
            {
                string oldName = Name;
                _dto.Name = cmd.NewName;
                AddEvent(new ConfigRenamedEvent(_dto.Id, oldName, _dto.Name));
            }
        }
    }
}
