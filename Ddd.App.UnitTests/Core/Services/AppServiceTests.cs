using Ddd.App.Core;
using Ddd.App.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.UnitTests.Core.Services
{
    public class AppServiceTests
    {
        public const string UnitTestDomain = "UnitTestDomain";
        public const string UnitTestSubDomain = "UnitTestSubDomain";
        private readonly IConfiguration _configuration;
        private readonly IOptions<AppConfig> _appConfig;
        private readonly AppService _appService;
        public AppServiceTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { AppService.AppDomainEnvironmentVariableName, UnitTestDomain },
                    { AppService.AppSubdomainEnvironmentVariableName, UnitTestSubDomain }
                })
                .Build();
            _appConfig = Options.Create(new AppConfig());
            _appService = new AppService(_configuration, _appConfig);
        }
        [Fact]
        public void GetDomain()
        {
            // Act
            var domain = _appService.Domain;
            // Assert
            Assert.Equal(UnitTestDomain, domain);
        }
        [Fact]
        public void GetSubDomain()
        {
            // Act
            var subDomain = _appService.SubDomain;
            // Assert
            Assert.Equal(UnitTestSubDomain, subDomain);
        }
        [Fact]
        public void GetVariables()
        {
            var variables = AppService.GetVariables(_configuration);
            Assert.Equal(2, variables.Count);
            var checkVariable = variables.Find(v => v.Name == AppService.AppDomainEnvironmentVariableName);
            Assert.NotNull(checkVariable);
            Assert.Equal(UnitTestDomain, checkVariable.Value);
            checkVariable = variables.Find(v => v.Name == AppService.AppSubdomainEnvironmentVariableName);
            Assert.NotNull(checkVariable);
            Assert.Equal(UnitTestSubDomain, checkVariable.Value);
        }
        [Fact]
        public void SubstituteVariableValues()
        {
            // Arrange
            var input = "The domain is ${APP_DOMAIN} and the subdomain is ${APP_SUBDOMAIN}.";
            var expectedOutput = $"The domain is {UnitTestDomain} and the subdomain is {UnitTestSubDomain}.";
            // Act
            var output = _appService.SubstituteVariableValues(input);
            // Assert
            Assert.Equal(expectedOutput, output);
        }
    }
}
