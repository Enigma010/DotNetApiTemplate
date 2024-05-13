using DotNetEventBus;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace EventBus.Di
{
    /// <summary>
    /// Dependency injection for the event bus
    /// </summary>
    public static class DependencyInjector
    {
        /// <summary>
        /// Environment variable name for the event bus host
        /// </summary>
        public const string HostEnvironmentVariableName = "EVENT_BUS_HOST";
        /// <summary>
        /// Environment variable name for the event bus user name
        /// </summary>
        public const string UsernameEnvironmentVariableName = "EVENT_BUS_USERNAME";
        /// <summary>
        /// Environment variable name for the event bus password
        /// </summary>
        public const string PasswordEnvironmentVariableName = "EVENT_BUS_PASSWORD";
        /// <summary>
        /// The default host for the event bus
        /// </summary>
        public const string DefaultHost = "host.docker.internal";
        /// <summary>
        /// The default user name
        /// </summary>
        public const string DefaultUsername = "guest";
        /// <summary>
        /// The default password
        /// </summary>
        public const string DefaultPassword = "guest";
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        /// <param name="useInMemory">Whether to use the in-memory event bus</param>
        public static void AddEventBusDependencies(this IHostApplicationBuilder builder, List<string> entryAssemblies, bool useInMemory = false)
        {
            builder.Services.AddMassTransit(x =>
            {
                string host = GetEnvironmentVariableOrDefault(HostEnvironmentVariableName, DefaultHost);
                string username = GetEnvironmentVariableOrDefault(UsernameEnvironmentVariableName, DefaultUsername);
                string password = GetEnvironmentVariableOrDefault(PasswordEnvironmentVariableName, DefaultPassword);
                x.SetKebabCaseEndpointNameFormatter();
                entryAssemblies.ForEach(entryAssemblyName =>
                {
                    Assembly entryAssembly = Assembly.Load(entryAssemblyName);
                    x.AddConsumers(entryAssembly);
                    x.AddActivities(entryAssembly);
                });
                if (useInMemory)
                {
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
                }
                else
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(host, "/", h =>
                        {
                            h.Username(username);
                            h.Password(password);
                        });
                        cfg.ConfigureEndpoints(context);
                    });
                }
            });
            builder.Services.AddScoped<IEventPublisher, EventPublisher>();
        }
        private static string GetEnvironmentVariableOrDefault(string environmentVariableName, string defaultValue)
        {
            string? value = Environment.GetEnvironmentVariable(environmentVariableName);
            return !string.IsNullOrEmpty(value) ? value : defaultValue;
        }
    }
}
