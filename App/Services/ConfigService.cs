using App.Commands;
using App.Entities;
using App.Repositories;
using App.Repositories.Dtos;
using DotNetApiAppCore.Services;
using App.Db;
using DotNetApiEventBus;
using DotNetApiLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App.UnitOfWork;

namespace App.Services
{
    /// <summary>
    /// Configuration service interface, defines what actions you can directly do with a
    /// configuration object
    /// </summary>
    public interface IConfigService : IBaseService<Config, Guid>
    {
        Task<Config> CreateAsync(ConfigCreateCmd cmd);
        Task<Config> GetAsync(Guid id);
        Task<IEnumerable<Config>> GetAsync(Paging paging);
        Task DeleteAsync(Guid id);
        Task<Config> RenameAsync(Guid id, ConfigRenameCmd cmd);
        Task<Config> EnablementAsync(Guid id, ConfigEnablementCmd cmd);
    }
    /// <summary>
    /// The configuration service.
    /// </summary>
    public class ConfigService : BaseService<IConfigRepository, Config, ConfigDto, Guid>, IConfigService
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
        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformationCaller("DeleteAsync: id: {@id}", args: [id]);
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
            {
                await unitOfWorks.RunAsync(async () =>
                {
                    Config config = await _repository.GetAsync(id);
                    await _repository.DeleteAsync(config);
                    await PublishEvents(config);
                });
            }
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        public async Task<Config> GetAsync(Guid id)
        {
            _logger.LogInformationCaller("GetAsync: id: {@id}", args: [id]);
            return await _repository.GetAsync(id);
        }

        /// <summary>
        /// Gets all of the configurations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Config>> GetAsync([FromQuery] Paging paging)
        {
            _logger.LogInformationCaller("DeleteAsync: paging: {@paging}", args: [paging]);
            return await _repository.GetAsync(paging);
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
            return await RunCommandAsync(id, nameof(RenameAsync), changeFunc);
        }

        /// <summary>
        /// Changes or updates a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <param name="cmd">The change that is occurring</param>
        /// <returns>The updated configuration</returns>
        public async Task<Config> EnablementAsync(Guid id, ConfigEnablementCmd cmd)
        {
            Func<Config, Config> changeFunc = (config) =>
            {
                config.Enablement(cmd);
                return config;
            };
            return await RunCommandAsync(id, nameof(EnablementAsync), changeFunc);
        }
    }
}
