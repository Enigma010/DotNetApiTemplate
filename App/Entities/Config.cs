using App.Commands;
using App.Repositories.Dtos;
using DotNetApiAppCore;
using AppEvents;

namespace App.Entities
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
        /// The ID of the configuration
        /// </summary>
        public Guid Id
        {
            get
            {
                return _dto.Id;
            }
        }
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string Name
        {
            get
            {
                return _dto.Name;
            }
        }

        /// <summary>
        /// Whether the configuration is active or not
        /// </summary>
        public bool Enabled 
        {
            get
            {
                return _dto.Enabled;
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
        /// <param name="change">The configuration changes</param>
        public void Change(ChangeConfigCmd change)
        {
            // Support for idempotence, this is done for future support for
            // eventing, i.e. if you event on the name change then if you 
            // call this method with the same name you don't want to trigger
            // a name change event
            string oldName = Name;
            bool oldEnabled = Enabled;
            bool changed = false;
            if (Name != change.Name)
            {
                _dto.Name = change.Name;
                changed = true;
            }
            if (Enabled != change.Enabled)
            {
                _dto.Enabled = change.Enabled;
                changed = true;
            }
            if(changed)
            {
                AddEvent(new ConfigChangedEvent(_dto.Id, oldName, _dto.Name, oldEnabled, _dto.Enabled));
            }
        }
    }
}
