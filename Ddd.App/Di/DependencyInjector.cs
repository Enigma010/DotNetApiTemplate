using Ddd.App.Repositories;
using Ddd.App.Services;
using DotNetApiAppCore;
using DotNetApiEventBus.Di;
using DotNetApiLogging.Di;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using DotNetApiLogging;
using Microsoft.Extensions.Configuration;
using Ddd.App.Core;
using Ddd.App.DbMongo.Di;
using Ddd.App.UnitOfWork;
using Ddd.App.Core.Services;

namespace Ddd.App.Di
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
            LogConfig logConfig = builder.Configuration.GetSection(nameof(LogConfig)).Get<LogConfig>() ?? throw new InvalidOperationException($"Missing {nameof(LogConfig)} configuration");
            builder.AddEventBusDependencies(["Ddd.App"]);
            builder.AddLoggerDependencies(logConfig);
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
            builder.Services.AddScoped<IEventPublisherUnitOfWork, EventPublisherUnitOfWork>();
            builder.Services.AddScoped<IAppService, AppService>();
            builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(AppConfig.ConfigurationSectionName));
        }
    }
}
