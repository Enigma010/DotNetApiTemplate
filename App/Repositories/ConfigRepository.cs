using App.Entities;
using Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    /// <summary>
    /// Configuration repository saves configuration options to the data store
    /// </summary>
    public interface IConfigRepository : IBaseRepository<Config, Guid>
    {
    }
    /// <summary>
    /// Configuration object for saving data to the data store
    /// </summary>
    public class ConfigRepository : BaseRepository<IConfigRepository, Config, Guid>, IConfigRepository
    {
        public ConfigRepository(IDbClient client, ILogger<IConfigRepository> logger)
            : base(client, logger)
        {
        }
    }
}
