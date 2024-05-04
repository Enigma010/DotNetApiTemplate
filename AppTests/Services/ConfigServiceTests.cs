using Amazon.Runtime.Internal.Util;
using App.ActionModels;
using App.Entities;
using App.Repositories;
using App.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTests.Services
{
    public class ConfigServiceTests
    {
        private readonly Mock<IConfigRepository> _repository;
        private readonly Mock<ILogger<IConfigService>> _logger;
        private readonly ConfigService _service;
        public ConfigServiceTests()
        {
            _repository = new Mock<IConfigRepository>();
            _logger = new Mock<ILogger<IConfigService>>();
            _service = new ConfigService(_repository.Object, _logger.Object);
        }
        [Fact]
        public async Task CreateAsync() 
        {
            string name = Guid.NewGuid().ToString();
            Config createConfig = await _service.CreateAsync(name);
            Assert.Equal(name, createConfig.Name);
            _repository.Verify(m => m.InsertAsync(It.IsAny<Config>()), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync()
        {
            Config config = new Config();
            await _service.DeleteAsync(config.Id);
            _repository.Verify(m => m.DeleteAsync(config.Id), Times.Once);
        }
        [Fact]
        public async Task GetAsync()
        {
            Config config = new Config();
            _repository.Setup(m => m.GetAsync(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config);
            Config getConfig = await _service.GetAsync(config.Id);
            Assert.Equal(config.Id, getConfig.Id);
            _repository.Verify(m => m.GetAsync(config.Id), Times.Once);
        }
        [Fact]
        public async Task GetAllAsync()
        {
            List<Config> configs = new List<Config>()
            {
                new Config(),
                new Config(),
                new Config(),
                new Config()
            };
            _repository.Setup(m => m.GetAsync()).ReturnsAsync(configs);
            IEnumerable<Config> getConfigs = await _service.GetAsync();
            Assert.Equal(configs.Count, getConfigs.Count());
            _repository.Verify(m => m.GetAsync(), Times.Once);
        }
        [Fact]
        public async Task ChangeAsync()
        {
            Config config = new Config();
            ConfigChangeActionModel model = new ConfigChangeActionModel()
            {
                Name = Guid.NewGuid().ToString(),
                Enabled = false
            };
            _repository.Setup(m => m.GetAsync(It.Is<Guid>(id => id == config.Id))).ReturnsAsync(config);
            _repository.Setup(m => m.UpdateAsync(It.Is<Config>(c => c.Id == config.Id))).ReturnsAsync(config);
            Config changeConfig = await _service.ChangeAsync(config.Id, model);
            Assert.Equal(config.Id, changeConfig.Id);
            Assert.Equal(model.Name, changeConfig.Name);
            Assert.Equal(model.Enabled, changeConfig.Enabled);
            _repository.Verify(m => m.UpdateAsync(It.Is<Config>(c => c.Id == config.Id)), Times.Once);
        }
    }
}
