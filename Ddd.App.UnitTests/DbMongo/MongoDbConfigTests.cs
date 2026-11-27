using Ddd.App.DbMongo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.UnitTests.DbMongo
{
    public class MongoDbConfigTests
    {
        private readonly IConfiguration _configuration;
        public MongoDbConfigTests()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { MongoDbConfig.UsernameKey, "testuser" },
                { MongoDbConfig.PasswordKey, "testpassword" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        [Fact]
        public void Test_MongoDbConfig_Returns_Correct_Values()
        {
            var config = new MongoDbConfig(_configuration);
            Assert.Equal("testuser", config.Username);
            Assert.Equal("testpassword", config.Password);
        }
    }
}
