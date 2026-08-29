using Ddd.App.Entities;
using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ddd.App.Core.Services
{
    public interface IAppService
    {
        public string Domain { get; }
        public string SubDomain { get; }
        public AppConfig AppConfig { get; }
        public string? SubstituteVariableValues(string? value);
    }
    public class AppService : IAppService
    {
        /// <summary>
        /// The name of the environment variable that holds the application domain.
        /// </summary>
        public const string AppDomainEnvironmentVariableName = "APP_DOMAIN";
        /// <summary>
        /// The name of the environment variable that holds the application subdomain.
        /// </summary>
        public const string AppSubdomainEnvironmentVariableName = "APP_SUBDOMAIN";
        private readonly IConfiguration _configuration;

        public AppService(IConfiguration configuration, 
            IOptions<AppConfig> appConfig)
        {
            _configuration = configuration;
            AppConfig = appConfig.Value;
        }
        public enum Config
        {
            [Config<string>(AppDomainEnvironmentVariableName)]
            Domain,
            [Config<string>(AppSubdomainEnvironmentVariableName)]
            SubDomain
        }
        public string Domain => Config.Domain.GetRequiredValue<string>(_configuration) ?? string.Empty;
        public string SubDomain => Config.SubDomain.GetRequiredValue<string>(_configuration) ?? string.Empty;
        public AppConfig AppConfig { get; private set; }
        /// <summary>
        /// Substitutes variable values in a template string with the corresponding environment variable values.
        /// </summary>
        /// <param name="template">The template string with variable placeholders</param>
        /// <returns>The string with variable values substituted</returns>
        public string? SubstituteVariableValues(string? template)
        {
            List<Variable> variables = new List<Variable>()
            {
                new Variable() { Name = AppDomainEnvironmentVariableName, Value = Domain },
                new Variable() { Name = AppSubdomainEnvironmentVariableName, Value = SubDomain }
            };
            return variables.Substitute(template);
        }
    }
}
