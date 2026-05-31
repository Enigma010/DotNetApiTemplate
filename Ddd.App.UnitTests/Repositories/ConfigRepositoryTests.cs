using Ddd.App.Db;
using Ddd.App.Entities;
using Ddd.App.Events;
using Ddd.App.Repositories;
using Ddd.App.Repositories.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace AppTests.Repositories
{
    public class ConfigRepositoryTests
    {
        private readonly Mock<ILogger<IConfigRepository>> _logger;
        private readonly Mock<IDbClient> _client;
        private ConfigRepository _repository;
        public ConfigRepositoryTests()
        {
            _logger = new Mock<ILogger<IConfigRepository>>();
            _client = new Mock<IDbClient>();
            _repository = new ConfigRepository(_client.Object, _logger.Object);
        }
        [Fact]
        public async Task InsertAsync()
        {
            Config config = new Config();
            Config changeConfig = await _repository.InsertAsync(config);
            Assert.Equal(config.Id, changeConfig.Id);
            _client.Verify(m => m.InsertAsync<ConfigDto, Guid>(config.GetDto()), Times.Once());
        }
        [Fact]
        public async Task InsertExistingAsync()
        {
            Config config = new Config();
            SetupGetAsync(config);
            await Assert.ThrowsAsync<DbEntityMultipleSingletonsException<Config>>(() => _repository.InsertAsync(config));
        }
        [Fact]
        public async Task GetAsync()
        {
            Config config = new Config();
            SetupGetAsync(config);
            Config getConfig = await _repository.GetAsync();
            Assert.Equal(config.Id, getConfig.Id);
            Assert.Empty(getConfig.GetEvents());
            _client.Verify(m => m.GetAsync<ConfigDto, Guid>(It.IsAny<Paging>(),
                It.IsAny<Expression<Func<ConfigDto, object>>?>()), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            Config config = new Config();
            Config updateConfig = await _repository.UpdateAsync(config);
            Assert.Equal(config.Id, updateConfig.Id);
            _client.Verify(m => m.UpdateAsync<ConfigDto, Guid>(config.GetDto()), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            ConfigRepository repository = new ConfigRepository(_client.Object, _logger.Object);
            Config config = new Config();
            config.ClearEvents();
            await repository.DeleteAsync(config);
            _client.Verify(m => m.DeleteAsync<ConfigDto, Guid>(It.Is<ConfigDto>(c => c.Id == config.Id)), Times.Once);
            Assert.Collection(
                config.GetEvents(),
                (c) =>
                {
                    Assert.IsType<ConfigDeletedEvent>(c);
                });
        }
        private void SetupGetAsync(Config config)
        {
            _client.Setup(m => m.GetAsync<ConfigDto, Guid>(
                It.IsAny<Paging>(),
                It.IsAny<Expression<Func<ConfigDto, object>>?>()))
            .ReturnsAsync(new List<ConfigDto>() { config.GetDto() });
        }
    }
}
