using App.Repositories;
using App.Services;
using DotNetApiAppCore;
using DotNetApiEventBus.Di;
using DotNetApiLogging.Di;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetApiMongoDb.Di;
using System.Diagnostics.CodeAnalysis;
using DotNetApiLogging;
using Microsoft.Extensions.Configuration;

namespace App.Di
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjector
    {
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        public static void AddAppDependencies(this IHostApplicationBuilder builder)
        {
            builder.AddMongoDbDependencies();
            AppConfig appConfig = new AppConfig(builder.Configuration);
            LogConfig logConfig = builder.Configuration.GetSection(nameof(LogConfig)).Get<LogConfig>() ?? throw new InvalidOperationException($"Missing {nameof(LogConfig)} configuration");
            builder.AddEventBusDependencies(["AppEventSubscribers"]);
            builder.AddLoggerDependencies(logConfig);
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
        }
    }
}
