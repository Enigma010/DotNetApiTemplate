using Ddd.App.Core.Services;
using Ddd.App.Web.Entities;
using DotNetApiEventBusCore;
using Microsoft.Extensions.Options;

namespace Ddd.App.Web.Services
{
    public class WebAuthenticationService
    {
        public const string AuthSecretKey = "AUTH_CLIENT_SECRET";

        private readonly IConfiguration _configuration;
        private readonly IOptions<WebAuthentication> _webAuthentication;
        private readonly IAppService _appService;
        public WebAuthenticationService(IConfiguration configuration,
            IOptions<WebAuthentication> webAuthentication,
            IAppService appService)
        {
            _configuration = configuration;
            _webAuthentication = webAuthentication;
            _appService = appService;
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
        public string ClientSecret
        {
            get
            {
                return Configs.WebClientSecret.GetRequiredValue<string>(_configuration);
            }
        }
        public string Authority
        {
            get
            {
                return _appService.SubstituteVariableValues(Authentication.Authority) ?? string.Empty;
            }
        }
        public string CookieName
        {
            get
            {
                return _appService.SubstituteVariableValues(Authentication.CookieName) ?? string.Empty;
            }
        }   
    }
}
