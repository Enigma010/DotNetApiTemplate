using Ddd.App.Core;
using Ddd.App.Db;
using DotNetApiEventBus;
using DotNetApiLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Ddd.App.DbMongo
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class MongoDbClient : IDbClient
    {
        /// <summary>
        /// The URI where MongoDB is located
        /// </summary>
        private readonly string _uri = string.Empty;
        /// <summary>
        /// The MongoDB client
        /// </summary>
        private readonly MongoClient _client;
        /// <summary>
        /// The client session
        /// </summary>
        private IClientSessionHandle? _session;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// The domain driven design configurations
        /// </summary>
        private readonly IDddConfig _dddConfig;
        /// <summary>
        /// Provides access to the MongoDB configuration settings used by the application.
        /// </summary>
        private readonly IMongoDbConfig _mongoDbConfig;
        /// <summary>
        /// Gets a value indicating whether scoped transactions are enabled for operations.
        /// </summary>
        public bool UseScopedTransactions => false;

        /// <summary>
        /// Creates a new MongoDB client.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <exception cref="NullReferenceException">Thrown if configuration is missing</exception>
        public MongoDbClient(IConfiguration configuration, ILogger<MongoDbClient> logger)
        {
            _uri = configuration.GetSection("Db")["Uri"] ?? throw new NullReferenceException("Missing Db.Uri in the configuration");
            _dddConfig = new DddConfig(configuration);
            _mongoDbConfig = new MongoDbConfig(configuration);
            _uri = _uri.Replace(IDbClient.UserNamePattern, Uri.EscapeDataString(_mongoDbConfig.Username));
            _uri = _uri.Replace(IDbClient.PasswordPattern, Uri.EscapeDataString(_mongoDbConfig.Password));
            _uri = _uri.Replace(IDbClient.DatabaseNamePattern, Uri.EscapeDataString(GetDatabaseName()));
            var settings = MongoClientSettings.FromConnectionString(_uri);
            if (_uri.Contains("tlsAllowInvalidHostnames=true"))
            {
                settings.SslSettings = new SslSettings
                {
                    ServerCertificateValidationCallback = CustomCertificateValidationCallback
                };
            }
            _client = new MongoClient(settings);
            _logger = logger;
        }
        /// <summary>
        /// Inserts an entity into MongoDB
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <typeparam name="IdType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>
        {
            _logger.LogInformationCaller("InsertAsync {@entity}", args: [entity]);
            if (entity == null)
            {
                throw new ArgumentNullException("Null entities cannot be saved");
            }
            if (_session == null)
            {
                throw new InvalidOperationException("Begin transatino must be called first");
            }
            var collection = GetCollectionForEntityType<EntityType>();
            _logger.LogInformation("Inserting {Id}", entity.Id);
            await collection.InsertOneAsync(_session, entity);
            _logger.LogInformation("Inserted {Id}", entity.Id);
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of the entities ID</typeparam>
        /// <param name="entity">The entity</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the entity is null</exception>
        public async Task UpdateAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType> where IdType : IComparable
        {
            _logger.LogInformationCaller("UpdateAsync {@entity}", args: [entity]);
            if (entity == null)
            {
                throw new ArgumentNullException("Null entities cannot be saved");
            }
            var collection = GetCollectionForEntityType<EntityType>();
            var filter = IdFilter<EntityType, IdType>(entity.Id);
            _logger.LogInformation("Replacing {Id}", entity.Id);
            await collection.ReplaceOneAsync<EntityType>(_session, doc => doc.Id.Equals(entity.Id), entity);
            _logger.LogInformation("Replaced {Id}", entity.Id);
        }

        /// <summary>
        /// Deletes and entity
        /// </summary>
        /// <typeparam name="EntityType">The entity to delete</typeparam>
        /// <typeparam name="IdType">The ID of the entity</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync<EntityType, IdType>(EntityType entity) where EntityType : IDbEntity<IdType>
        {
            _logger.LogInformationCaller("DeleteAsync {@entity}", args: [entity]);
            if (_session == null)
            {
                throw new InvalidOperationException("Begin transatino must be called first");
            }
            var collection = GetCollectionForEntityType<EntityType>();
            var filter = IdFilter<EntityType, IdType>(entity.Id);
            _logger.LogInformation("Deleting {Id}", entity.Id);
            await collection.DeleteOneAsync(_session, filter);
            _logger.LogInformation("Deleted {Id}", entity.Id);
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        /// <typeparam name="EntityType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of the ID</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns>The entity</returns>
        /// <exception cref="DbEntityNotFoundException{IdType}">Thrown if the entity isn't found</exception>
        public async Task<EntityType> GetAsync<EntityType, IdType>(IdType id)
        {
            _logger.LogInformationCaller("GetAsync {@id}", args: [id]);
            var collection = GetCollectionForEntityType<EntityType>();
            var filter = IdFilter<EntityType, IdType>(id);
            _logger.LogInformation("Getting {Id}", id);
            var entities = await (await collection.FindAsync<EntityType>(filter)).ToListAsync();
            _logger.LogInformation("Got {Id}", id);
            if (!entities.Any())
            {
                throw new DbEntityNotFoundException<IdType>(id);
            }
            return entities.First();
        }

        /// <summary>
        /// Gets and entity based on an expression
        /// </summary>
        /// <typeparam name="EntityType">The entity type</typeparam>
        /// <param name="expression">The expression to get the entity by</param>
        /// <returns></returns>
        public async Task<IEnumerable<DbType>> GetAsync<DbType, IdType>(Expression<Func<DbType, bool>> expression, Paging paging, Expression<Func<DbType, object>>? sort = null) where DbType : IDbEntity<IdType>
        {
            Expression<Func<DbType, object>> sortBy = GetSortBy<DbType, IdType>(sort);
            _logger.LogInformationCaller("GetAsync expression: {@expression}, paging: {@paging}, sortBy: {@sortBy}", args: [expression, paging, sortBy]);
            var collection = GetCollectionForEntityType<DbType>();
            var filter = Builders<DbType>.Filter.Where(expression);
            _logger.LogInformation("Getting by expression");
            var entities = await collection.Find(filter)
                .SortBy(sortBy)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Limit(paging.PageSize)
                .ToListAsync();
            _logger.LogInformation("Got by expression");
            return entities;
        }

        /// <summary>
        /// Gets all the entities
        /// </summary>
        /// <typeparam name="DbType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of ID</typeparam>
        /// <returns>The entities</returns>
        public async Task<IEnumerable<DbType>> GetAsync<DbType, IdType>(Paging paging, Expression<Func<DbType, object>>? sort = null) where DbType : IDbEntity<IdType>
        {
            Expression<Func<DbType, object>> sortBy = GetSortBy<DbType, IdType>(sort);
            _logger.LogInformationCaller("GetAsync paging: {@paging}, sortBy: {@sortBy}", args: [paging, sortBy]);
            var collection = GetCollectionForEntityType<DbType>();
            var filter = Builders<DbType>.Filter.Empty;
            _logger.LogInformation("Getting all");
            var entities = await collection.Find(filter)
                .SortBy(sortBy)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Limit(paging.PageSize)
                .ToListAsync();
            _logger.LogInformation("Got all");
            return entities;
        }

        /// <summary>
        /// Begins a unit of work
        /// </summary>
        /// <returns></returns>
        public async Task Begin()
        {
            if (_session == null)
            {
                _session = await _client.StartSessionAsync();
            }
            _session.StartTransaction();
        }

        /// <summary>
        /// Ends a unit of work
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if begin unit of work wasn't called</exception>
        public async Task Commit()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Commit cannot be called without a call to Begin");
            }
            if (_session.IsInTransaction)
            {
                await _session.CommitTransactionAsync();
            }
        }

        /// <summary>
        /// Rolls back a unit of work
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Rollback()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Rollback cannot be called without a call to Begin");
            }
            if (_session.IsInTransaction)
            {
                _session.AbortTransaction();
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets the database name based on the configuration
        /// </summary>
        /// <returns>The database name</returns>
        public string GetDatabaseName()
        {
            return $"{_dddConfig.Domain}-{_dddConfig.SubDomain}";
        }

        /// <summary>
        /// Resolves a C# class to a MongoDB collection
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <returns></returns>
        private IMongoCollection<EntityType> GetCollection<EntityType>()
        {
            return _client.GetDatabase(GetDatabaseName()).GetCollection<EntityType>(typeof(EntityType).Name);
        }

        /// <summary>
        /// Gets a filter clause base on the ID
        /// </summary>
        /// <typeparam name="DbType">The type of entity</typeparam>
        /// <typeparam name="IdType">The type of ID</typeparam>
        /// <param name="id">The ID</param>
        /// <returns></returns>
        private FilterDefinition<DbType> IdFilter<DbType, IdType>(IdType id)
        {
            return Builders<DbType>.Filter.Eq("_id", id);
        }
        /// <summary>
        /// Logs the message before we get the collection name
        /// </summary>
        /// <typeparam name="DbType"></typeparam>
        private IMongoCollection<DbType> GetCollectionForEntityType<DbType>()
        {
            _logger.LogInformation("Getting collection for {EntityName}", typeof(DbType).Name);
            IMongoCollection<DbType> collection = GetCollection<DbType>();
            _logger.LogInformation("Got collection for {EntityName} is {CollectionName}", typeof(DbType).Name, collection.CollectionNamespace.CollectionName);
            return collection;
        }

        /// <summary>
        /// Gets the sort to use
        /// </summary>
        /// <typeparam name="DbType">The database type</typeparam>
        /// <typeparam name="IdType">The ID type</typeparam>
        /// <param name="sort">What to sort by</param>
        /// <returns>The sort operator to use</returns>
        private Expression<Func<DbType, object>> GetSortBy<DbType, IdType>(Expression<Func<DbType, object>>? sort = null) where DbType : IDbEntity<IdType>
        {
            Expression<Func<DbType, object>> defaultSort = (e) => e.Id;
            return sort ?? defaultSort;
        }

        // Custom validation callback method
        private bool CustomCertificateValidationCallback(
            object sender,
            X509Certificate? certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // If there are no errors, the certificate is valid
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNotAvailable))
            {
                return false;
            }

            // Check if the only error is a RemoteCertificateNameMismatch
            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors))
            {
                // Log the error for debugging if needed, but return true to allow the connection
                _logger.LogInformationCaller("Hostname mismatch ignored: " + sslPolicyErrors);
                return true;
            }

            // For any other errors (e.g., certificate not trusted, expired), fail the validation
            _logger.LogErrorCaller("Certificate validation error: " + sslPolicyErrors);
            return false;
        }
    }
}
