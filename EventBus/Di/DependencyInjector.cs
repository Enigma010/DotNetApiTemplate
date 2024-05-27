using DotNetEventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using static MassTransit.MessageHeaders;

namespace EventBus.Di
{
    /// <summary>
    /// Dependency injection for the event bus
    /// </summary>
    public static class DependencyInjector
    {
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
        public static void AddEventBusDependencies(this IHostApplicationBuilder builder, List<string> entryAssemblies)
        {
            IConfigurationSection section = builder.Configuration.GetSection("EventBus");
            string host = section["Host"] ?? DefaultHost;
            string username = section["Username"] ?? DefaultUsername;
            string password = section["Password"] ?? DefaultPassword;
            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                entryAssemblies.ForEach(entryAssemblyName =>
                {
                    Assembly entryAssembly = Assembly.Load(entryAssemblyName);
                    x.AddConsumers(entryAssembly);
                    x.AddActivities(entryAssembly);
                });
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            builder.Services.AddScoped<IEventPublisher, EventPublisher>();
            builder.Services.AddScoped((sp) =>
            {
                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                });
            });
        }
    }
}
