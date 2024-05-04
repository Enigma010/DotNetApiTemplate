﻿using App.Entities;
using App.Repositories;
using Db;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task ChangeAsync()
        {
            Config config = new Config();
            Config changeConfig = await _repository.InsertAsync(config);
            Assert.Equal(config.Id, changeConfig.Id);
            _client.Verify(m => m.InsertAsync<Config, Guid>(config), Times.Once());
        }
        [Fact]
        public async Task GetAsync()
        {
            Config config = new Config();
            _client.Setup(m => m.GetAsync<Config, Guid>(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config);
            Config changeConfig = await _repository.InsertAsync(config);
            Assert.Equal(config.Id, changeConfig.Id);
            Config getConfig = await _repository.GetAsync(config.Id);
            Assert.Equal(config.Id, getConfig.Id);
            _client.Verify(m => m.GetAsync<Config, Guid>(config.Id), Times.Once);
        }
        [Fact]
        public async Task GetAllAsync()
        {
            List<Config> configs = new List<Config>()
            {
                new Config(),
                new Config(),
                new Config()
            };
            _client.Setup(m => m.GetAsync<Config, Guid>()).ReturnsAsync(configs);
            IEnumerable<Config> getConfigs = await _repository.GetAsync();
            Assert.Equal(configs.Count, getConfigs.Count());
            _client.Verify(m => m.GetAsync<Config, Guid>(), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            Config config = new Config();
            Config updateConfig = await _repository.UpdateAsync(config);
            Assert.Equal(config.Id, updateConfig.Id);
            _client.Verify(m => m.UpdateAsync<Config, Guid>(config), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            ConfigRepository repository = new ConfigRepository(_client.Object, _logger.Object);
            Config config = new Config();
            await repository.DeleteAsync(config.Id);
            _client.Verify(m => m.DeleteAsync<Config, Guid>(It.Is<Guid>(id => id == config.Id)), Times.Once);
        }
    }
}