using Ddd.App.Core.Repositories;
using Ddd.App.Db;
using Ddd.App.Entities;
using Ddd.App.Repositories.Dtos;
using Microsoft.Extensions.Logging;

namespace Ddd.App.Repositories
{
    /// <summary>
    /// Configuration repository saves configuration options to the data store
    /// </summary>
    public interface IConfigRepository : IBaseSingletonRepository<Config, ConfigDto, Guid>
    {
    }
    /// <summary>
    /// Configuration object for saving data to the data store
    /// </summary>
    public class ConfigRepository : BaseSingletonRepository<IConfigRepository, Config, ConfigDto, Guid>, IConfigRepository
    {
        public ConfigRepository(IDbClient client, ILogger<IConfigRepository> logger)
            : base(client, logger)
        {
        }
    }
}
