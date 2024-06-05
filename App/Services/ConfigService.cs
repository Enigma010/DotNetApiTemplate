using App.Commands;
using App.Entities;
using App.Repositories;
using AppCore.Services;
using EventBus;
using Microsoft.Extensions.Logging;
using UnitOfWork;

namespace App.Services
{
    /// <summary>
    /// Configuration service interface, defines what actions you can directly do with a
    /// configuration object
    /// </summary>
    public interface IConfigService : IBaseService<Config, Guid>
    {
        Task<Config> CreateAsync(CreateConfigCmd cmd);
        Task<Config> GetAsync(Guid id);
        Task<IEnumerable<Config>> GetAsync();
        Task DeleteAsync(Guid id);
        Task<Config> ChangeAsync(Guid id, ChangeConfigCmd cmd);
    }
    /// <summary>
    /// The configuration service.
    /// </summary>
    public class ConfigService : BaseService<IConfigRepository, Config, Guid>, IConfigService
    {
        /// <summary>
        /// Logger object
        /// </summary>
        private readonly ILogger<IConfigService> _logger;

        /// <summary>
        /// Creates a configuration service
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="logger">The logger</param>
        public ConfigService(
            IConfigRepository repository, 
            ILogger<IConfigService> logger,
            IEventPublisher eventPublisher) 
            : base(repository, eventPublisher)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a new configuration with all of the defaults
        /// </summary>
        /// <param name="cmd">The create config command</param>
        /// <returns>The new configuration object</returns>
        public async Task<Config> CreateAsync(CreateConfigCmd cmd)
        {
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks))
            {
                return await unitOfWorks.RunAsync(async () =>
                {
                    Config config = new Config();
                    config.Change(
                        new ChangeConfigCmd()
                        {
                            Name = cmd.Name
                        });
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
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks))
            {
                await unitOfWorks.RunAsync(async () =>
                {
                    Config config = await _repository.GetAsync(id);
                    await _repository.DeleteAsync(config);
                    await PublishEvents(config);
                    await unitOfWorks.Commit();
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
            return await _repository.GetAsync(id);
        }

        /// <summary>
        /// Gets all of the configurations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Config>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        /// <summary>
        /// Changes or updates a configuration
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <param name="change">The change that is occurring</param>
        /// <returns>The updated configuration</returns>
        public async Task<Config> ChangeAsync(Guid id, ChangeConfigCmd change)
        {
            using (var unitOfWorks = new UnitOfWorks(_unitOfWorks))
            {
                return await unitOfWorks.RunAsync(async () =>
                {
                    Func<Config, Config> changeFunc = (config) =>
                    {
                        config.Change(change);
                        return config;
                    };
                    var config = await ChangeAsync(id, changeFunc);
                    await PublishEvents(config);
                    return config;
                });
            }
        }
    }
}
