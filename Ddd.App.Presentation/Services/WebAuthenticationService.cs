using Ddd.App.Presentation.Entities;
using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ddd.App.Presentation.Services
{
    public class WebAuthenticationService
    {
        public const string AuthSecretKey = "AUTH_CLIENT_SECRET";

        private readonly IConfiguration _configuration;
        private readonly IOptions<WebAuthentication> _webAuthentication;
        public WebAuthenticationService(IConfiguration configuration,
            IOptions<WebAuthentication> webAuthentication)
        {
            _configuration = configuration;
            _webAuthentication = webAuthentication;
        }
        public enum Configs
        {
            [Config<string>(AuthSecretKey)]
            WebClientSecret
        }
        public WebAuthentication Authentication
        {
            get
            {
                return _webAuthentication.Value;
            }
        }
        public string ClientSecret => GetClientSecret(_configuration);
        public static string GetClientSecret(IConfiguration configuration)
        {
            return Configs.WebClientSecret.GetRequiredValue<string>(configuration);
        }
    }
}
