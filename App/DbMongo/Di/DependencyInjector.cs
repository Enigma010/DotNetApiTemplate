using App.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Diagnostics.CodeAnalysis;

namespace App.DbMongo.Di
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would be in a different project")]
    public static class DependencyInjector
    {
        /// <summary>
        /// Does dependency injection for MongoDB
        /// </summary>
        /// <param name="builder">The host application builder</param>
        public static void AddMongoDbDependencies(this IHostApplicationBuilder builder)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddScoped<IDbClient, MongoDbClient>();
        }
    }
}
