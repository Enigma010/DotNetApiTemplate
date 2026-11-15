using Ddd.App.Commands;
using Ddd.App.Db;
using Ddd.App.Entities;
using Ddd.App.Repositories;
using Ddd.App.Repositories.Dtos;
using Ddd.App.UnitOfWork;
using DotNetApiAppCore.Services;
using DotNetApiLogging;
using Microsoft.Extensions.Logging;

namespace Ddd.App.Services
{
    /// <summary>
    /// Configuration service interface, defines what actions you can directly do with a
    /// configuration object
    /// </summary>
    public interface IConfigService : IBaseSingletonService<Config, Guid>
    {
        Task<Config> CreateAsync(ConfigCreateCmd cmd);
        Task<Config> GetAsync();
        Task DeleteAsync();
        Task<Config> RenameAsync(Guid id, ConfigRenameCmd cmd);
        Task<Config> GetOrCreateAsync();
    }
    /// <summary>
    /// The configuration service.
    /// </summary>
    public class ConfigService : BaseSingletonService<IConfigRepository, Config, ConfigDto, Guid>, IConfigService
    {
        /// <summary>
        /// Creates a configuration service
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="logger">The logger</param>
        public ConfigService(
            IConfigRepository repository,
            ILogger<IConfigService> logger,
            IEventPublisherUnitOfWork eventPublisher)
            : base(repository, eventPublisher, logger)
        {
        }

        /// <summary>
        /// Gets or creates the configuration
        /// </summary>
        /// <returns>The configuration</returns>
        public async Task<Config> GetOrCreateAsync()
        {
            _logger.LogInformationCaller($"GetorCreateAsync");
            try
            {
                return await GetAsync();
            }
            catch (DbSingletonEntityNotFoundException)
            {
                return await CreateAsync(ConfigCreateCmd.Default());
            }
        }

        /// <summary>
        /// Creates a new configuration with all of the defaults
        /// </summary>
        /// <param name="cmd">The create config command</param>
        /// <returns>The new configuration object</returns>
        public async Task<Config> CreateAsync(ConfigCreateCmd cmd)
        {
            _logger.LogInformationCaller("CreateAsync: command {@cmd}", args: [cmd]);
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
            {
                return await unitOfWorks.RunAsync(async () =>
                {
                    Config config = new Config(cmd.Name);
                    await _repository.InsertAsync(config);
                    await PublishEvents(config);
                    return config;
                });
            }
        }

        /// <summary>
        /// Deletes a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            _logger.LogInformationCaller("DeleteAsync");
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
            {
                await unitOfWorks.RunAsync(async () =>
                {
                    Config? config = await _repository.GetAsync();
                    if(config != null)
                    {
                        await _repository.DeleteAsync(config);
                        await PublishEvents(config);
                    }
                });
            }
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        public async Task<Config> GetAsync()
        {
            _logger.LogInformationCaller("GetAsync");
            return await _repository.GetAsync();
        }

        /// <summary>
        /// Changes or updates a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <param name="cmd">The change that is occurring</param>
        /// <returns>The updated configuration</returns>
        public async Task<Config> RenameAsync(Guid id, ConfigRenameCmd cmd)
        {
            Func<Config, Config> changeFunc = (config) =>
            {
                config.Rename(cmd);
                return config;
            };
            return await RunCommandAsync(nameof(RenameAsync), changeFunc);
        }
    }
}
