using Db;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SharpCompress.Common;
using System.Linq.Expressions;

namespace MongoDb
{
    public class MongoDbClient : IDbClient
    {
        /// <summary>
        /// The default database name, if not specified in the configuration this value will be used
        /// </summary>
        public const string DefaultDatabase = "db";
        /// <summary>
        /// The URI where MongoDB is located
        /// </summary>
        private readonly string _uri = string.Empty;
        /// <summary>
        /// The database to use
        /// </summary>
        private readonly string _database = DefaultDatabase;
        /// <summary>
        /// The MongoDB client
        /// </summary>
        private readonly MongoClient _client;
        /// <summary>
        /// Creates a new MongoDB client.  The following configurations are required:
        ///   MONGO_USERNAME: The username to use to login to MongoDB
        ///   MONGO_PASSWORD: The password to use to login to MongoDB
        ///   *** Note that for development you can set these as environment variables and they
        ///   *** will be respected
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="NullReferenceException">Throw if configuration is missing</exception>
        public MongoDbClient(IConfiguration configuration)
        {
            _uri = configuration.GetSection("Db")["Uri"] ?? throw new NullReferenceException("Missing Db:Uri in the configuration");
            _uri = _uri.Replace(IDbClient.UserNamePattern, Uri.EscapeDataString(configuration["MONGO_USERNAME"] ?? throw new NullReferenceException("Missing MONGO_USERNAME in the configuration")));
            _uri = _uri.Replace(IDbClient.PasswordPattern, Uri.EscapeDataString(configuration["MONGO_PASSWORD"] ?? throw new NullReferenceException("Missing MONGO_PASSWORD in the configuration")));
            _database = configuration.GetSection("Db")["Database"] ?? throw new NullReferenceException("Missing Db:DatabaseName in the configuration");
            _client = new MongoClient(_uri);
        }

        /// <summary>
        /// Inserts an entity into MongoDB
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <typeparam name="IdType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertAsync<EntityType, IdType>(EntityType entity) where EntityType : IEntity<IdType>
        {
            if(entity == null)
            {
                throw new ArgumentNullException("Null entities cannot be saved");
            }
            var collection = GetCollection<EntityType>();
            await collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync<EntityType, IdType>(EntityType entity) where EntityType : IEntity<IdType> where IdType : IComparable
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Null entities cannot be saved");
            }
            var collection = GetCollection<EntityType>();
            var filter = IdFilter<EntityType, IdType>(entity.Id);
            await collection.ReplaceOneAsync<EntityType>(doc => doc.Id.Equals(entity.Id), entity);
        }

        public async Task DeleteAsync<EntityType, IdType>(EntityType entity) where EntityType : IEntity<IdType>
        {
            var collection = GetCollection<EntityType>();
            var filter = IdFilter<EntityType, IdType>(entity.Id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<EntityType> GetAsync<EntityType, IdType>(IdType id)
        {
            var collection = GetCollection<EntityType>();
            var filter = IdFilter<EntityType, IdType>(id);
            var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
            if (!entities.Any())
            {
                throw new DbEntityNotFoundException<IdType>(id);
            }
            return entities.First();
        }

        public async Task<IEnumerable<EntityType>> GetAsync<EntityType>(Expression<Func<EntityType, bool>> expression)
        {
            var collection = GetCollection<EntityType>();
            var filter = Builders<EntityType>.Filter.Where(expression);
            var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<EntityType>> GetAsync<EntityType, IdType>()
        {
            var collection = GetCollection<EntityType>();
            var filter = Builders<EntityType>.Filter.Empty;
            var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
            return entities;
        }

        private IMongoCollection<EntityType> GetCollection<EntityType>()
        {
            return _client.GetDatabase(_database).GetCollection<EntityType>(typeof(EntityType).Name);
        }

        private FilterDefinition<EntityType> IdFilter<EntityType, IdType>(IdType id)
        {
            return Builders<EntityType>.Filter.Eq("_id", id);
        }
    }
}
