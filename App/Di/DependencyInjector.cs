using App.Repositories;
using App.Services;
using EventBus.Di;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDb;
using System.Diagnostics.CodeAnalysis;

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
            builder.AddEventBusDependencies(new List<string>() { "AppEventConsumers" });
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
        }
    }
}
