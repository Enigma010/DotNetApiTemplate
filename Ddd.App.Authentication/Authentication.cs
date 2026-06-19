using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;

namespace Ddd.App.Authentication
{
    public class Authentication
    {
        public const string AuthSecretKey = "AUTH_CLIENT_SECRET";
        public const string SectionName = nameof(Authentication);
        public Authentication()
        {
        }
        public enum Configs
        {
            [Config<string>(AuthSecretKey)]
            WebClientSecret
        }
        public string Authority { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string CookieName { get; set; } = "myapp.auth";
        public string LogoutUrl { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ClientSecret(IConfiguration configuration)
        {
            return Configs.WebClientSecret.GetRequiredValue<string>(configuration);
        }
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
