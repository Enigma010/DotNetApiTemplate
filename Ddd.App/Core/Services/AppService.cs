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
        public string Domain => GetDomain(_configuration);
        public string SubDomain => GetSubDomain(_configuration);
        public AppConfig AppConfig { get; private set; }
        /// <summary>
        /// Gets the domain value
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>The domain</returns>
        public static string GetDomain(IConfiguration configuration)
        {
            return Config.Domain.GetRequiredValue<string>(configuration) ?? string.Empty;
        }
        /// <summary>
        /// Gets the subdomain
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>The subdomain</returns>
        public static string GetSubDomain(IConfiguration configuration)
        {
            return Config.SubDomain.GetRequiredValue<string>(configuration) ?? string.Empty;
        }
        /// <summary>
        /// Gets the variables for the app service
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns>The list of variables</returns>
        public static List<Variable> GetVariables(IConfiguration configuration)
        {
            return GetVariables(GetDomain(configuration), GetSubDomain(configuration));
        }
        /// <summary>
        /// Gets the list of variables with their corresponding values for the given domain and subdomain.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="subDomain"></param>
        /// <returns>The list of variables</returns>
        public static List<Variable> GetVariables(string domain, string subDomain)
        {
            return new List<Variable>()
            {
                new Variable() { Name = AppDomainEnvironmentVariableName, Value = domain },
                new Variable() { Name = AppSubdomainEnvironmentVariableName, Value = subDomain }
            };
        }
        /// <summary>
        /// Substitutes variable values in a template string with the corresponding environment variable values.
        /// </summary>
        /// <param name="template">The template</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The string with variable values substituted</returns>
        public static string? SubstituteVariableValues(string? template, IConfiguration configuration)
        {
            List<Variable> variables = GetVariables(configuration);
            return variables.Substitute(template);
        }
        /// <summary>
        /// Substitutes variable values in a template string with the corresponding environment variable values.
        /// </summary>
        /// <param name="template">The template string with variable placeholders</param>
        /// <returns>The string with variable values substituted</returns>
        public string? SubstituteVariableValues(string? template)
        {
            List<Variable> variables = GetVariables(Domain, SubDomain);
            return variables.Substitute(template);
        }
    }
}
