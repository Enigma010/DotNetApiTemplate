using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace App.Core
{
    public static class EnvVars
    {
        public enum DddVars
        {
            [EnvVar(name: "APP_DOMAIN")]
            Domain,
            [EnvVar(name: "APP_SUBDOMAIN")]
            Subdomain,
        }

        public enum DbVars
        {
            [EnvVar(name: "APP_DB_USERNAME")]
            UserName,
            [EnvVar(name: "APP_DB_PASSWORD")]
            Password
        }

        public static string? GetName(this Enum enumValue)
        {
            var attr = enumValue.GetEnvVar();
            return attr?.Name;
        }

        public static string GetRequiredName(this Enum enumValue)
        {
            var attr = enumValue.GetEnvVar();
            if (attr == null)
            {
                throw new InvalidOperationException($"The enum value {enumValue} does not have an associated EnvVarAttribute.");
            }
            return attr.Name;
        }

        public static EnvVarType GetRequiredValue<EnvVarType>(this Enum enumValue, IConfiguration configurationManager)
        {
            string envVarName = enumValue.GetRequiredName();
            return GetValue<EnvVarType>(enumValue, configurationManager) ?? throw new InvalidOperationException($"The environment variable {envVarName} is not set.");
        }

        public static EnvVarType? GetValue<EnvVarType>(this Enum enumValue, IConfiguration configurationManager)
        {
            string envVarName = enumValue.GetRequiredName();
            return configurationManager.GetValue<EnvVarType>(envVarName);
        }

        public static EnvVarAttribute? GetEnvVar(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);

            if (name == null)
                return null;

            var field = type.GetField(name);
            return field?.GetCustomAttribute<EnvVarAttribute>();
        }
    }
}
