using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public class AppConfig
    {
        public const string ConfigurationSectionName = "App";
        public const string NameSectionName = "Name";
        public AppConfig(IConfigurationManager configurationManager) 
        {
            IConfigurationSection section = configurationManager.GetSection(ConfigurationSectionName);
            Name = section[NameSectionName] ?? string.Empty;
        }
        public string Name { get; private set; } = string.Empty;
    }
}
