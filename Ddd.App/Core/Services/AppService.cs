using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ddd.App.Core.Services
{
    public interface IAppService
    {
        public string Domain { get; }
        public string SubDomain { get; }
        public AppConfig AppConfig { get; }
    }
    public class AppService : IAppService
    {
        public const string AppDomainEnvironmentVariableName = "APP_DOMAIN";
        public const string AppSubdomainEnvironmentVariableName = "APP_SUBDOMAIN";
        private readonly IConfiguration _configuration;
        public AppService(IConfiguration configuration, IOptions<AppConfig> appConfig)
        {
            _configuration = configuration;
            AppConfig = appConfig.Value;
        }
        public string Domain => _configuration.GetValue<string>(AppDomainEnvironmentVariableName) ?? string.Empty;
        public string SubDomain => _configuration.GetValue<string>(AppSubdomainEnvironmentVariableName) ?? string.Empty;
        public AppConfig AppConfig { get; private set; }

    }
}
