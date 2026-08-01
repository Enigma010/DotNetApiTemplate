using System.Diagnostics.CodeAnalysis;

namespace Ddd.App.Core
{
    /// <summary>
    /// The application configurations
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class AppConfig
    {
        public const string ConfigurationSectionName = "App";
        public const string NameSectionName = "Name";
        public const string DefaultAppName = "Default App Name";
        /// <summary>
        /// Creates a new app configuration object
        /// </summary>
        /// <param name="configurationManager"></param>
        public AppConfig()
        {
        }
        /// <summary>
        /// The name of the application
        /// </summary>
        public string Name { get; private set; } = DefaultAppName;
    }
}
