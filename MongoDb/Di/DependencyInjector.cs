using Db;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MongoDb.Di
{
    public static class DependencyInjector
    {
        /// <summary>
        /// Does dependency injection for MongoDB
        /// </summary>
        /// <param name="builder">The host application builder</param>
        public static void AddMongoDbDependencies(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDbClient, MongoDbClient>();
        }
    }
}
