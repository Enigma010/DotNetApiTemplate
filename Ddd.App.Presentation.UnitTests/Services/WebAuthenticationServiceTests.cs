using Ddd.App.Presentation.Entities;
using Ddd.App.Presentation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ddd.App.Presentation.UnitTests.Services
{
    public class WebAuthenticationServiceTests
    {
        public const string TestSecret = "test-securet";
        private readonly WebAuthenticationService _webAuthenticationService;

        public WebAuthenticationServiceTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { WebAuthenticationService.AuthSecretKey, TestSecret }
                }).Build();
            IOptions<WebAuthentication> webAuthenticationOptions = Options.Create(new WebAuthentication()
            {
                Authority = nameof(WebAuthentication.Authority),
                ClientId = nameof(WebAuthentication.ClientId),
                CookieName = nameof(WebAuthentication.CookieName),
                LogoutUrl = nameof(WebAuthentication.LogoutUrl),
                ValidIssuer = nameof(WebAuthentication.ValidIssuer),
                ValidAudience = nameof(WebAuthentication.ValidAudience),
                RequireHttpsMetadata = true,
                ShowPII = false
            });
            _webAuthenticationService = new WebAuthenticationService(configuration, webAuthenticationOptions);
        }

        [Fact]
        public void GetClientSecret_ShouldReturnExpectedValue()
        {
            // Act
            string clientSecret = _webAuthenticationService.ClientSecret;
            // Assert
            Assert.Equal(TestSecret, clientSecret);
        }
        [Fact]
        public void GetWebAuthentication_ShouldReturnExpectedValue()
        {
            // Act
            WebAuthentication webAuthentication = _webAuthenticationService.Authentication;
            // Assert
            Assert.Equal(nameof(WebAuthentication.Authority), webAuthentication.Authority);
            Assert.Equal(nameof(WebAuthentication.ClientId), webAuthentication.ClientId);
            Assert.Equal(nameof(WebAuthentication.CookieName), webAuthentication.CookieName);
            Assert.Equal(nameof(WebAuthentication.LogoutUrl), webAuthentication.LogoutUrl);
            Assert.Equal(nameof(WebAuthentication.ValidIssuer), webAuthentication.ValidIssuer);
            Assert.Equal(nameof(WebAuthentication.ValidAudience), webAuthentication.ValidAudience);
            Assert.True(webAuthentication.RequireHttpsMetadata);
            Assert.False(webAuthentication.ShowPII);
        }
    }
}
