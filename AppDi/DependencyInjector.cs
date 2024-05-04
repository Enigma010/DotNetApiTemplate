using App.Repositories;
using App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDb;

namespace AppDi
{
    public static class DependencyInjector
    {
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        public static void AddAppDependencies(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
            builder.AddMongoDbDependencies();
        }
    }
}
