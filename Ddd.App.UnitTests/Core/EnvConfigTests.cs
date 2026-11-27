using Ddd.App.Core;
using Microsoft.Extensions.Configuration;

namespace Ddd.App.UnitTests.Core
{
    public class EnvConfigTests
    {
        public const string TestPath = "testpath";
        private readonly IConfiguration _configuration;
        public EnvConfigTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { $"{EnvConfig.EnvConfigSectionName}:{nameof(EnvConfig.Path)}", TestPath }
                })
                .Build();
        }
        [Fact]
        public void Test_EnvConfig_Returns_Correct_Path()
        {
            var config = new EnvConfig();
            _configuration.Bind(EnvConfig.EnvConfigSectionName, config);
            Assert.Equal(TestPath, config.Path);
        }
    }
}
