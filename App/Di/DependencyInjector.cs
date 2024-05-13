using App.Repositories;
using App.Services;
using EventBus.Di;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDb;

namespace App.Di
{
    public static class DependencyInjector
    {
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        public static void AddAppDependencies(this IHostApplicationBuilder builder)
        {
            builder.AddMongoDbDependencies();
            builder.AddEventBusDependencies(new List<string>() { "Api" });
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
        }
    }
}
