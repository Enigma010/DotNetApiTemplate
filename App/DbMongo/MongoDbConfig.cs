using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.DbMongo
{
    /// <summary>
    /// Represents the configuration settings required to connect to a MongoDB database, including credentials such as
    /// username and password.
    /// </summary>
    /// <remarks>This class provides strongly-typed access to MongoDB authentication settings, which are
    /// typically sourced from environment variables or configuration files. The constant fields define the expected
    /// environment variable keys for these settings.</remarks>
    public class MongoDbConfig : IMongoDbConfig
    {
        /// <summary>
        /// Represents the configuration key used to retrieve the application's database username.
        /// </summary>
        public const string UsernameKey = "APP_DB_USERNAME";
        /// <summary>
        /// Represents the configuration key used to retrieve the application's database password.
        /// </summary>
        public const string PasswordKey = "APP_DB_PASSWORD";
        /// <summary>
        /// Specifies the available configuration keys for user authentication settings.
        /// </summary>
        /// <remarks>Use the members of this enumeration to reference specific configuration values, such
        /// as the username or password, when accessing or storing authentication-related settings. The associated
        /// attributes provide metadata for each configuration key.</remarks>
        public enum Configs
        {
            [Config<string>(name: UsernameKey)]
            Username,
            [Config<string>(name: PasswordKey)]
            Password
        }
        /// <summary>
        /// Gets or sets the username associated with the user account.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the password associated with the current object.
        /// </summary>
        public string Password { get; set; } = string.Empty;
        public MongoDbConfig(IConfiguration configuration)
        {
            Username = Configs.Username.GetRequiredValue<string>(configuration);
            Password = Configs.Password.GetRequiredValue<string>(configuration);
        }
    }
}
