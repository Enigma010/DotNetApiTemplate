using Ddd.App.Web.Services;

namespace Ddd.App.Web.Entities
{
    public class WebAuthentication
    {
        public const string SectionName = nameof(WebAuthentication);
        public string Authority { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string CookieName { get; set; } = string.Empty;
        public string LogoutUrl { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
    }
}
